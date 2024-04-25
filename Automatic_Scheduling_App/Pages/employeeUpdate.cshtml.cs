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
        public string manager { get; set; }
        public string signin { get; set; }
        public string updated { get; set; }
        public string updateFail { get; set; }

        public employeeUpdateModel(ILogger<employeeUpdateModel> logger)
        {
            _logger = logger;
            user_id = 0;
            database = new MySqlConnection(db_config);

            signin = "LogOut";
            manager = "block";

            userData = new Dictionary<string, string> 
            {
                {"dept_name", "Unknown Department" },
                {"position", "Unknown Role" },
                {"email", "Unknown Username" },

                {"first_name", "N/A" },
                {"last_name", "N/A" },
                {"ssn", "N/A" },
                {"dob", "N/A" },

                {"number", "N/A" },
                {"type", "N/A" },
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

        private void GetUserData()
        {
            try
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
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
                    userData["dept_name"] = reader.GetString("dept_name");
                    userData["position"] = reader.GetString("position");
                    userData["email"] = reader.GetString("email");

                    userData["first_name"] = reader.GetString("first_name");
                    userData["last_name"] = reader.GetString("last_name");
                    userData["ssn"] = reader.GetString("ssn");
                    DateOnly dob = reader.GetDateOnly("date_of_birth");
                    userData["dob"] = dob.ToString();

                    userData["number"] = reader.GetString("phone_number");
                    userData["type"] = reader.GetString("phone_type");
                    userData["gender"] = reader.GetString("gender");

                    userData["street"] = reader.GetString("street");
                    userData["city"] = reader.GetString("city");
                    userData["unit"] = reader.GetString("unit");
                    userData["state"] = reader.GetString("state");
                    userData["zipcode"] = reader.GetString("zipcode");

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

                select.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                updated = "block";
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

                // no need to access database if user does not exists
                return;
            }

            // begin user data collection
            string value;

            value = Request.Form["first_name"];
            if (userData["first_name"] != value)
                Console.WriteLine("first name is " + value);
                //UpdateUserRecord("first_name", value);

            value = Request.Form["last_name"];
            if (userData["last_name"] == value)
                Console.WriteLine("last name is " + value);
            //UpdateUserRecord("last_name", value);

            value = Request.Form["number"];
            if (userData["number"] != value)
                //UpdateUserRecord("phone_number", value);

            /*

            DateOnly dob = reader.GetDateOnly("date_of_birth");
            userData["dob"] = dob.ToString();

            userData["number"] = reader.GetString("phone_number");
            userData["type"] = reader.GetString("phone_type");
            userData["gender"] = reader.GetString("gender");

            userData["street"] = reader.GetString("street");
            userData["city"] = reader.GetString("city");
            userData["unit"] = reader.GetString("unit");
            userData["state"] = reader.GetString("state");
            userData["zipcode"] = reader.GetString("zipcode");

            */

            HttpContext.Session.SetString("updated", "block");
            HttpContext.Session.SetString("updateFail", "none");
        }

        public IActionResult OnGet()
        {
            try
            {
                // get the state of the Data updates flag 
                updated = HttpContext.Session.GetString("updated");
            }
            catch
            {
                updated = "none";
            }
            GetUserData();
            
            return Page();
        }
        public IActionResult OnPost()
        {
            UpdateUserData();

            return RedirectToPage();
        }
    }
}
