using Automatic_Scheduling_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace Automatic_Scheduling_App.Pages
{
    public class employeeUpdateModel : PageModel
    {
        private readonly ILogger<employeeUpdateModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        public Dictionary<string, string> stateInput { get; set; }
        public Dictionary<string, string> userData { get; set; }

        private int user_id;
        private int reset;
        private bool errorFound;
        public string manager { get; set; }
        private int manager_id;
        public string userValid { get; set; }
        public string signin { get; set; }
        public string updated { get; set; }
        public string updateFail { get; set; }
        public int notify { get; set; }

        public employeeUpdateModel(ILogger<employeeUpdateModel> logger)
        {
            _logger = logger;
            user_id = 0;
            database = new MySqlConnection(db_config);

            signin = "LogOut";
            manager = "none";
            userValid = "block";

            errorFound = false;

            userData = new Dictionary<string, string> 
            {
                {"dept_name", "Unknown Department" },
                {"position", "Unknown Role" },
                {"email", "Unknown Username" },

                {"first_name", "N/A" },
                {"last_name", "N/A" },
                {"ssn", "N/A" },
                {"dob", "N/A" },

                {"phone_number", "N/A" },
                {"phone_type", "N/A" },
                {"gender", "O" },

                {"street", "N/A" },
                {"city", "N/A" },
                {"unit", "N/A" },
                {"state", "N/A" },
                {"zipcode", "N/A" }
            };

            stateInput = new Dictionary<string, string>
            {
                {"Alabama", "AL"},{"Alaska", "AK"},{"Arizona", "AZ"},{"Arkansas", "AR"},{"California", "CA"},{"Colorado", "CO"},
                {"Connecticut", "CT"},{"Delaware", "DE"},{"Florida", "FL"},{"Georgia", "GA"},{"Hawaii", "HI"},{"Idaho", "ID"},
                {"Illinois", "IL"},{"Indiana", "IN"},{"Iowa", "IA"},{"Kansas", "KS"},{"Kentucky", "KY"},{"Louisiana", "LA"},
                {"Maine", "ME"},
                {"Maryland", "MD"},
                {"Massachusetts", "MA"},
                {"Michigan", "MI"},
                {"Minnesota", "MN"},
                {"Mississippi", "MS"},
                {"Missouri", "MO"},
                {"Montana", "MT"},
                {"Nebraska", "NE"},
                {"Nevada", "NV"},
                {"New Hampshire", "NH"},
                {"New Jersey", "NJ"},
                {"New Mexico", "NM"},
                {"New York", "NY"},
                {"North Carolina", "NC"},
                {"North Dakota", "ND"},
                {"Ohio", "OH"},
                {"Oklahoma", "OK"},
                {"Oregon", "OR"},
                {"Pennsylvania", "PA"},
                {"Rhode Island", "RI"},
                {"South Carolina", "SC"},
                {"South Dakota", "SD"},
                {"Tennessee", "TN"},
                {"Texas", "TX"},
                {"Utah", "UT"},
                {"Vermont", "VT"},
                {"Virginia", "VA"},
                {"Washington", "WA"},
                {"West Virginia", "WV"},
                {"Wisconsin", "WI"},
                {"Wyoming", "WY"}
            };
        }

        private void UpdateNotify()
        {
            try
            {
                database.Open();
                string query = "select count(*) from time_off_request " +
                                "where user_id = @UserID and apprv > 0 and dayoff > CURDATE()";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@UserID", user_id);
                object result = select.ExecuteScalar();

                if (result != null)
                    notify = Convert.ToInt32(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }
        private void GetUserData()
        {
            try
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
                manager_id = (int)HttpContext.Session.GetInt32("manager_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
                manager_id = 0;
                // no need to access database if user does not exists
                return;
            }

            // begin user data collection

            try
            {
                database.Open();
                string query = "select staff.*, dept_name from staff natural inner join department where user_id = @userID";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@userID", user_id);

                MySqlDataReader reader = select.ExecuteReader();

                // Iterate through the result set
                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("dept_name")))
                    {
                        userData["dept_name"] = reader.GetString("dept_name");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("position")))
                    {
                        userData["position"] = reader.GetString("position");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("email")))
                    {
                        userData["email"] = reader.GetString("email");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("first_name")))
                    {
                        userData["first_name"] = reader.GetString("first_name");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("last_name")))
                    {
                        userData["last_name"] = reader.GetString("last_name");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("ssn")))
                    {
                        userData["ssn"] = reader.GetString("ssn");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("date_of_birth")))
                    {
                        DateOnly dob = reader.GetDateOnly("date_of_birth");
                        userData["dob"] = dob.ToString();
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("phone_number")))
                    {
                        userData["phone_number"] = reader.GetString("phone_number");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("phone_type")))
                    {
                        userData["phone_type"] = reader.GetString("phone_type");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("gender")))
                    {
                        userData["gender"] = reader.GetString("gender");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("street")))
                    {
                        userData["street"] = reader.GetString("street");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("city")))
                    {
                        userData["city"] = reader.GetString("city");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("unit")))
                    {
                        userData["unit"] = reader.GetString("unit");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("state")))
                    {
                        userData["state"] = reader.GetString("state");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("zipcode")))
                    {
                        userData["zipcode"] = reader.GetString("zipcode");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private void UpdateUserRecord(string attribute, string value)
        {
            try
            {
                database.Open();
                string query = "update staff set " +
                                attribute + " = @Value where user_id = @userID";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@Value", value);
                select.Parameters.AddWithValue("@userID", user_id);

                select.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                updateFail = "block";
                updated = "none";
                errorFound = true;

                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private void UpdateUserData()
        {
            try
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;

                HttpContext.Session.SetString("updated", "none");
                HttpContext.Session.SetString("updateFail", "block");
                HttpContext.Session.SetInt32("eu_reset", 1);

                // no need to access database if user does not exists
                return;
            }

            // begin user data collection
            errorFound = false;
            string value;

            value = Request.Form["first_name"];
            if (userData["first_name"] != value)
                UpdateUserRecord("first_name", value);

            value = Request.Form["last_name"];
            if (userData["last_name"] != value)
                UpdateUserRecord("last_name", value);

            value = Request.Form["phone_number"];
            if (userData["phone_number"] != value)
                UpdateUserRecord("phone_number", value);

            value = Request.Form["phone_type"];
            if (userData["phone_type"] != value)
                UpdateUserRecord("phone_type", value);

            value = Request.Form["gender"];
            if (userData["gender"] != value)
                UpdateUserRecord("gender", value);

            value = Request.Form["street"];
            if (userData["street"] != value)
                UpdateUserRecord("street", value);

            value = Request.Form["city"];
            if (userData["city"] != value)
                UpdateUserRecord("city", value);

            value = Request.Form["unit"];
            if (userData["unit"] != value)
                UpdateUserRecord("unit", value);

            value = Request.Form["state"];
            if (userData["state"] != value)
                UpdateUserRecord("state", value);

            value = Request.Form["zipcode"];
            if (userData["zipcode"] != value)
                UpdateUserRecord("zipcode", value);

            if (errorFound)
            {
                HttpContext.Session.SetString("updated", "none");
                HttpContext.Session.SetString("updateFail", "block");
            }
            else
            {
                HttpContext.Session.SetString("updated", "block");
                HttpContext.Session.SetString("updateFail", "none");
            }

            HttpContext.Session.SetInt32("eu_reset", 1);
        }

        public IActionResult OnGet()
        {
            // set flag reset
            try
            {
                reset = (int)HttpContext.Session.GetInt32("eu_reset");
            }
            catch (Exception ex)
            {
                reset = 0;
            }

            // check flags to display
            try
            {
                if (reset != 0)
                {
                    // get the state of the Data updates flag 
                    updateFail = HttpContext.Session.GetString("updateFail");
                    updated = HttpContext.Session.GetString("updated");
                }
                else
                {
                    updated = "none";
                    updateFail = "none";
                }
                
            }
            catch (Exception ex)
            {
                updated = "none";
                updateFail = "none";
            }

            GetUserData();

            // try sent user back to index if he tries coming back here without login
            if (user_id == 0)
                return RedirectToPage("Index");

            if (manager_id > 0)
                manager = "block";

            UpdateNotify();

            return Page();
        }
        public IActionResult OnPost()
        {
            GetUserData();
            UpdateUserData();

            return RedirectToPage();
        }
    }
}
