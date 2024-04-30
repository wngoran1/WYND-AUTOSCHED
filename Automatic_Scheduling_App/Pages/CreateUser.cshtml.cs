using Automatic_Scheduling_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using System.Reflection.PortableExecutable;
using System;

namespace Automatic_Scheduling_App.Pages
{
    public class createUserModel : PageModel
    {
        private readonly ILogger<createUserModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        private int user_id;
        public string manager { get; set; }
        public string message { get; set; }
        public string signin { get; set; }
        public Dictionary<string, int> departments { get; set; }
        public Dictionary<string, string> register { get; set; }

        private bool errorlog;

        private Random random = new Random();

        public createUserModel(ILogger<createUserModel> logger)
        {
            _logger = logger;
            user_id = 0;
            errorlog = false;
            database = new MySqlConnection(db_config);

            signin = "LogOut";
            manager = "block";
            message = "";

            loadDepartments();
            loadRegistration();
        }

        private void loadDepartments()
        {
            departments = new Dictionary<string, int>();
            try
            {
                database.Open();
                string query = "select dept_id, dept_name from department order by dept_name";
                MySqlCommand select = new MySqlCommand(query, database);

                MySqlDataReader reader = select.ExecuteReader();
                // Iterate through the result set
                while (reader.Read())
                {
                    // update the list of departments
                    departments[reader.GetString("dept_name")] = reader.GetInt32("dept_id");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private void loadRegistration()
        {
            register = new Dictionary<string, string>();
            try
            {
                database.Open();
                string query = "select code_id, email from signupcode order by email";
                MySqlCommand select = new MySqlCommand(query, database);

                MySqlDataReader reader = select.ExecuteReader();
                // Iterate through the result set
                while (reader.Read())
                {
                    // update the list of departments
                    register[reader.GetString("email")] = reader.GetString("code_id");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private string PersonalCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

            StringBuilder codeBuilder = new StringBuilder();

            // Generate the first four characters (XXXX)
            for (int i = 0; i < 4; i++)
            {
                codeBuilder.Append(chars[random.Next(chars.Length)]);
            }

            // Add hyphen after the fourth character
            codeBuilder.Append("-");

            // Generate the next two characters (XX)
            for (int i = 0; i < 2; i++)
            {
                codeBuilder.Append(chars[random.Next(chars.Length)]);
            }

            // Add hyphen after the sixth character
            codeBuilder.Append("-");

            // Generate the last four characters (XXXX)
            for (int i = 0; i < 2; i++)
            {
                codeBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return codeBuilder.ToString();
        }

        private void RegisterNewUser(string email, string position, int dept_id, string code_id)
        {
            int user_id = 0;
            try
            {
                // Open the connection
                database.Open();
                string query = "select user_id from staff where email = @Email";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@Email", email);

                object result = select.ExecuteScalar();

                if (result != null)
                {
                    user_id = Convert.ToInt32(result);
                    message = "Email Already Exists";
                }

                else
                {
                    // SQL query to insert a new week tuple
                    string setQ = "INSERT INTO staff (email, position, dept_id) " +
                            "VALUES (@Email, @Role, @DeptID)";
                    MySqlCommand insert = new MySqlCommand(setQ, database);
                    insert.Parameters.AddWithValue("@Email", email);
                    insert.Parameters.AddWithValue("@Role", position);
                    insert.Parameters.AddWithValue("@DeptID", dept_id);
                    insert.ExecuteNonQuery();

                    // SQL query to retrieve the id of the last user created above
                    string getQ = "select user_id from staff where email = @Email";
                    MySqlCommand retrieve = new MySqlCommand(getQ, database);
                    retrieve.Parameters.AddWithValue("@Email", email);

                    user_id = (int)retrieve.ExecuteScalar();

                    // SQL query to insert new user in the signup table
                    string signQ = "INSERT INTO signupcode (user_id, email, code_id) " +
                            "VALUES (@UserID, @Email, @Code)";
                    MySqlCommand adduser = new MySqlCommand(signQ, database);
                    adduser.Parameters.AddWithValue("@UserID", user_id);
                    adduser.Parameters.AddWithValue("@Email", email);
                    adduser.Parameters.AddWithValue("@Code", code_id);
                    adduser.ExecuteNonQuery();

                    message = "New User Added";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                message = "Username already in Use";
            }
            finally
            {
                // Close connection
                database.Close();
            }
        }

        public IActionResult OnGet()
        {
            try // verify user
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
            }

            // try sent user back to index if he tries coming back here without login
            if (user_id == 0)
                return RedirectToPage("Index");

            return Page();
        }

        public void OnPost()
        {
            int dept_id = int.Parse(Request.Form["dept_id"]);
            string position = Request.Form["position"];
            string email = Request.Form["email"];
            string code_id = PersonalCode();

            if (dept_id == 0)
            {
                message = "Please Select a VALID Department";
            }
            else 
            {
                RegisterNewUser(email, position, dept_id, code_id);
                loadRegistration();
            }
        }
        
    }
}
