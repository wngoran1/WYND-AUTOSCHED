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

namespace Automatic_Scheduling_App.Pages
{
    public class employeeViewModel : PageModel
    {
        private readonly ILogger<employeeViewModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        private int user_id;
        private int staff_id;
        private List<int> deptEmployees;
        public List<string> departments { get; set; }
        public int index { get; set; }
        public int range { get; set; }
        public Dictionary<string, string> userData { get; set; }
        public string manager { get; set; }
        public string signin { get; set; }

        public employeeViewModel(ILogger<employeeViewModel> logger)
        {
            _logger = logger;
            user_id = 0;
            database = new MySqlConnection(db_config);

            signin = "LogOut";
            manager = "block";

            range = 0;
        }

        private void loadDepartments()
        {
            departments = []; // reset
            try
            {
                database.Open();
                string query = "select dept_name from department order by dept_name";
                MySqlCommand select = new MySqlCommand(query, database);

                MySqlDataReader reader = select.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    // update the list of departments
                    departments.Add(reader.GetString("dept_name"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }
        private void GetUserData(string dept_name)
        {
            userData = new Dictionary<string, string>
                {
                {"dept_name", dept_name },
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

            if (staff_id == 0) return; // no user selected

            try
            {
                database.Open();
                string query = "select staff.*, dept_name from staff natural inner join department where user_id = @userID";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@userID", staff_id);

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

                    userData["phone_number"] = reader.GetString("phone_number");
                    userData["phone_type"] = reader.GetString("phone_type");
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

        private void Download_employees_from(string department)
        {
            // List to store user IDs
            List<int> employees = [];

            try
            {

                // Open the connection
                database.Open();

                // SQL query to select staff IDs from the staff table
                string query = "SELECT user_id FROM staff natural inner join department " +
                            "where dept_name = @DeptName order by first_name";
                // query = query + department;
                // Console.WriteLine(query);
                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@DeptName", department);
                MySqlDataReader reader = command.ExecuteReader();


                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the staff_id to the list
                    employees.Add(Convert.ToInt32(reader["user_id"]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Close connection
                database.Close();
            }
            deptEmployees =  employees;
            range = employees.Count;
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

            try // retrieve last index traversal
            {
                index = (int)HttpContext.Session.GetInt32("index");
            }
            catch (Exception ex)
            {
                index = 0;
            }

            string dept_name;
            try // retrieve department to traverse
            {
                dept_name = HttpContext.Session.GetString("dept_name");
            }
            catch (Exception ex)
            {
                dept_name = "Unknown Department";
            }

            // begin update procedure
            loadDepartments();
            Download_employees_from(dept_name);

            if (range < 1)
            {
                staff_id = 0;
                index = 0;
                HttpContext.Session.SetInt32("index", index);
            }
                
            else
            {
                // pivot the out of bound index
                // update stored value
                if (index < 0)
                {
                    index = index + range;
                    HttpContext.Session.SetInt32("index", index);
                }
                else
                {
                    index = index % range;
                    HttpContext.Session.SetInt32("index", index);
                }
                // assign next employee to display
                staff_id = deptEmployees[index];
            }
                

            GetUserData(dept_name);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Request.Form.ContainsKey("dept_update"))
            {
                // update session variables for department and reset index
                HttpContext.Session.SetInt32("index", 0);
                string dept_name = Request.Form["dept_name"];
                HttpContext.Session.SetString("dept_name", dept_name);
            }
            else if (Request.Form.ContainsKey("prevbut"))
            {
                index = (int)HttpContext.Session.GetInt32("index");
                index--;
                // update stored value
                HttpContext.Session.SetInt32("index", index);
            }
            else if (Request.Form.ContainsKey("nextbut"))
            {
                index = (int)HttpContext.Session.GetInt32("index");
                index++;
                // update stored value
                HttpContext.Session.SetInt32("index", index);
            }

            /*foreach (var key in Request.Form.Keys)
            {
                var value = Request.Form[key];
                // Do something with the key-value pair
                Console.WriteLine($"Field '{key}': '{value}'");
            }*/

            return RedirectToPage();
        }
    }
}
