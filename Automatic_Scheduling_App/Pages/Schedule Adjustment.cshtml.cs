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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using System.Security.Cryptography.Xml;

namespace Automatic_Scheduling_App.Pages
{
    public class Schedule_AdjustmentModel : PageModel
    {
        private readonly ILogger<Schedule_AdjustmentModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";

        private MySqlConnection database;
        private int user_id;
        public int manager_id;

        public string signin { get; set; }
        public string manager { get; set; }
        public string userValid { get; set; }

        public List<Tuple<string, string, string, string, int>> record { get; set; }

        private List<int> userData;
        public string message;

        public Schedule_AdjustmentModel(ILogger<Schedule_AdjustmentModel> logger)
        {
            database = new MySqlConnection(db_config);
            _logger = logger;

            signin = "LogOut";
            manager = "none";
            userValid = "block";
            message = "";
        }

        private void loadAllRequestRecord()
        {
            record = [];
            userData = [];

            try
            {
                database.Open();
                string query = "select user_id, first_name, last_name, dayoff, reason, subdate, apprv from time_off_request " +
                                "NATURAL INNER JOIN staff WHERE apprv = 0";
                MySqlCommand select = new MySqlCommand(query, database);

                MySqlDataReader reader = select.ExecuteReader();

                while (reader.Read())
                {
                    userData.Add(reader.GetInt32("user_id"));
                    string username = reader.GetString("first_name") + " " + reader.GetString("last_name");

                    DateOnly dayoff = reader.GetDateOnly("dayoff");
                    DateOnly subdate = reader.GetDateOnly("subdate");
                    var newData = new Tuple<string, string, string, string, int>(username, dayoff.ToString(),
                                   reader.GetString("reason"), subdate.ToString(), reader.GetInt32("apprv"));
                    record.Add(newData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        public IActionResult OnGet()
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
            }

            // try sent user back to index if he tries coming back here without login
            // if (user_id == 0)
                //return RedirectToPage("Index");

            if (manager_id > 0)
                manager = "block";

            loadAllRequestRecord();

            return Page();
        }

        public void OnPost()
        {
        }
    }
}
