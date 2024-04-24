using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Automatic_Scheduling_App.Models;
using MySqlConnector;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace Automatic_Scheduling_App.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }

        public bool logerror { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            logerror = false;
        }

        private int GetLoginCredentials(string email, string pswd)
        {
            int user_id = -1;
            try
            {
                Console.WriteLine("STEP before open");
                database.Open();
                Console.WriteLine("STEP 0");
                string query = "select user_id from staff natural inner join credentials where email = @email and pswd = @pswd";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@email", email);
                select.Parameters.AddWithValue("@pswd", pswd);

                Console.WriteLine("STEP 1");
                object result = select.ExecuteScalar();
                Console.WriteLine("STEP 2");

                // Check if the result is not null and is convertible to int
                if (result != null && result != DBNull.Value)
                {
                    user_id = Convert.ToInt32(result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Login Error: " + ex.Message);
                // Handle exceptions
            }
            finally
            {
                database.Close(); // Close the database connection
            }

            return user_id;
        }

        public void OnGet()
        {
            HttpContext.Session.SetInt32("user_id", 0);
            HttpContext.Session.SetString("manager", "none");
        }

        public IActionResult OnPost()
        {
            int user_id = 0;
            string email = Request.Form["username"];
            string pswd = Request.Form["pswd"];

            user_id = GetLoginCredentials(email, pswd);

            if (user_id > 0)
            {
                HttpContext.Session.SetInt32("user_id", user_id);
                // return RedirectToPage("dashboard");
            }
            return RedirectToPage();
        }
    }
}
