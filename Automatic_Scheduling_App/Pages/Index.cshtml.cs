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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;"; 
        private MySqlConnection database { get; set; }
        public string logerror { get; set; }
        public string manager { get; set; }
        public string signin { get; set; }

        private int reset;
        public IndexModel(ILogger<IndexModel> logger)
        {
            signin = "WYND Solutions";
            _logger = logger;
            database = new MySqlConnection(db_config);
        }

        private int Download_user_id(string email, string pswd)
        {
            int user_id = -1;
            try
            {
                database.Open();
                string query = "select user_id from staff natural inner join credentials where email = @email and pswd = @pswd";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@email", email);
                select.Parameters.AddWithValue("@pswd", pswd);

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

        private void CheckLogInfo()
        {
            try
            {
                reset = (int)HttpContext.Session.GetInt32("reset");
            }
            catch (Exception ex)
            {
                reset = 0;
            }

            // this checks if the user entered the wrong credential
            // and dispay the error message when activated
            try
            {
                // we try first in case the page loads for the first time
                // and the error mesage toggler has not been set in session yet
                if (reset != 0)
                {
                    logerror = HttpContext.Session.GetString("error");
                    manager = HttpContext.Session.GetString("manager");
                }
                else
                {
                    manager = "none";
                    logerror = "none";
                }

            }
            catch (Exception ex)
            {
                manager = "none";
                logerror = "none";
            }
        }

        public void OnGet()
        {
            CheckLogInfo();
            HttpContext.Session.SetInt32("user_id", 0);
        }

        public IActionResult OnPost()
        {
            string email = Request.Form["email"];
            string pswd = Request.Form["pswd"];

            int user_id = Download_user_id(email,pswd);
            if (user_id > 0)
            {
                HttpContext.Session.SetInt32("user_id", user_id);
                HttpContext.Session.SetInt32("reset", 0);
                HttpContext.Session.SetString("error", "none");
                HttpContext.Session.SetString("manager", "none");
                return RedirectToPage("dashboard");
            }

            // warn user for incorrect login info
            HttpContext.Session.SetInt32("reset", 1);
            HttpContext.Session.SetString("error", "block");
            HttpContext.Session.SetString("manager", "none");
            return RedirectToPage();
        }
    }
}
