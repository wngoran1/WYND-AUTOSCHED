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
    public class employeeViewModel : PageModel
    {
        private readonly ILogger<employeeViewModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        public Dictionary<string, string> stateInput { get; set; }
        private int user_id;
        public string manager { get; set; }
        public string signin { get; set; }

        public employeeViewModel(ILogger<employeeViewModel> logger)
        {
            _logger = logger;
            user_id = 0;
            signin = "LogOut";
            manager = "block";

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
        
        private void SetUserData(int user_id)
        {
            List<string> user = [];
            try
            {
                database.Open();
                string query = "SELECT first_name, last_name, position, dept_name FROM staff NATURAL INNER JOIN department WHERE user_id = @user_id";
                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@user_id", user_id);

                MySqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the tupples to the list
                    user.Add(reader.GetString("first_name"));
                    user.Add(reader.GetString("last_name"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally
            {
                database.Close(); // Close the database connection
            }
            if (user.Count > 1)
            {
     
            }
            else
            {

            }
        }

        public void OnGet()
        {
            try
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
            }

            //SetUserData(user_id);
        }

        public void OnPost()
        {

        }
    }
}
