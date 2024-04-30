using Automatic_Scheduling_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Net.Http;
using Microsoft.Win32;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Automatic_Scheduling_App.Pages
{
    public class employeeSignUpModel : PageModel
    {
        private readonly ILogger<employeeSignUpModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        private int user_id;
        public string manager { get; set; }
        public string userpass { get; set; }
        public string useradd { get; set; }
        public string signin { get; set; }
        public string message { get; set; }
        public Dictionary<string, string> stateInput { get; set; }
        public Dictionary<string, string> userData { get; set; }

        public employeeSignUpModel(ILogger<employeeSignUpModel> logger)
        {
            
            _logger = logger;
            database = new MySqlConnection(db_config);
            user_id = 0;

            message = "";
            useradd = "none";
            userpass = "block";
            manager = "none";
            signin = "LogIn";

            userData = new Dictionary<string, string>
            {
                {"dept_name", "Unknown Department" },
                {"position", "Unknown Role" },
                {"email", "Unknown Username" }
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
                user_id = (int)HttpContext.Session.GetInt32("r_user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
                message = "We cannot validate your registration! contact your manager";
                useradd = "none";
                userpass = "block";
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private int ValidateUser(string email, string code_id)
        {
            int user_id = 0;
            try
            {
                database.Open();
                string query = "select user_id from signupcode where email = @Email and code_id = @Code";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@Email", email);
                select.Parameters.AddWithValue("@Code", code_id);

                MySqlDataReader reader = select.ExecuteReader();

                // Iterate through the result set
                if (reader.Read())
                {
                    user_id = Convert.ToInt32(reader["user_id"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally
            {
                database.Close();
                // Close the database connection
            }
            return user_id;
        }

        private void UpdateUserRecord(string attribute, string value)
        {
            if (user_id == 0) return;
            // no need to access database

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
                user_id = 0;
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private void UpdateUserData()
        {
            try
            {
                user_id = (int)HttpContext.Session.GetInt32("r_user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;

                // no need to access database if user does not exists
                return;
            }

            // begin user data collection
            string value;

            value = Request.Form["first_name"];
                UpdateUserRecord("first_name", value);

            value = Request.Form["last_name"];
                UpdateUserRecord("last_name", value);

            value = Request.Form["ssn"];
                UpdateUserRecord("ssn", value);

            // trick to handle the date
            value = Request.Form["date_of_birth"];
            DateOnly dob = DateOnly.Parse(value);
            value = dob.ToString("yyyy-MM-dd");
            UpdateUserRecord("date_of_birth", value);

            value = Request.Form["phone_number"];
                UpdateUserRecord("phone_number", value);

            value = Request.Form["phone_type"];
                UpdateUserRecord("phone_type", value);

            value = Request.Form["gender"];
                UpdateUserRecord("gender", value);

            value = Request.Form["street"];
                UpdateUserRecord("street", value);

            value = Request.Form["city"];
                UpdateUserRecord("city", value);

            value = Request.Form["unit"];
                UpdateUserRecord("unit", value);

            value = Request.Form["state"];
                UpdateUserRecord("state", value);

            value = Request.Form["zipcode"];
                UpdateUserRecord("zipcode", value);
        }

        private void SetupCredentials(string email, string pswd)
        {
            try
            {
                database.Open();

                // STEP 1: Insert new credentials
                string query = "insert into credentials (email, pswd) values (@Email, @Pass)";
                MySqlCommand insert = new MySqlCommand(query, database);
                insert.Parameters.AddWithValue("@Email", email);
                insert.Parameters.AddWithValue("@Pass", pswd);
                insert.ExecuteNonQuery();

                // STEP 2: Delete Registration code
                string deletequery = "delete from signupcode where email = @Email";
                MySqlCommand delete = new MySqlCommand(deletequery, database);
                delete.Parameters.AddWithValue("@Email", email);
                delete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (Request.Form.ContainsKey("register"))
            {
                // check registration code
                string email = Request.Form["email"];
                string code_id = Request.Form["code_id"];
                user_id = ValidateUser(email, code_id);

                if (user_id == 0)
                {
                    message = "We cannot validate your registration! contact your manager";
                }
                else
                {
                    // Check matching user password
                    string pswd = Request.Form["pswd"];
                    string pswd_check = Request.Form["pswd_check"];

                    if (pswd != pswd_check)
                    {
                        message = "Password does not match";
                    }
                    else
                    {
                        // temporarily save user input for next step
                        HttpContext.Session.SetInt32("r_user_id", user_id);
                        HttpContext.Session.SetString("email", email);
                        HttpContext.Session.SetString("pswd", pswd);

                        // set the cotainer flags
                        message = "";
                        useradd = "block";
                        userpass = "none";

                        // prepare next step initial data
                        GetUserData();
                    }
                }

            }
            else if (Request.Form.ContainsKey("createUser"))
            {
                UpdateUserData();

                if (user_id == 0)
                {
                    message = "An error occured during Registration! Try Again!";
                    useradd = "none";
                    userpass = "block";
                }
                else
                {
                    message = "";
                    useradd = "none";
                    userpass = "none";
                    
                    string email = HttpContext.Session.GetString("email");
                    string pswd = HttpContext.Session.GetString("pswd");
                    SetupCredentials(email, pswd);

                    return RedirectToPage("Index");
                }
            }

            return Page();
        }
    }
}
