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
    public class employeeTimeOffModel : PageModel
    {
        private readonly ILogger<employeeTimeOffModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";

        private MySqlConnection database;
        private int user_id;
        public int manager_id;

        public int notify { get; set; }
        public string signin { get; set; }
        public string manager { get; set; }
        public string userValid { get; set; }

        public List<Tuple<string, string, string, int>> record { get; set; }
        public string message;

        public employeeTimeOffModel(ILogger<employeeTimeOffModel> logger)
        {
            database = new MySqlConnection(db_config);
            _logger = logger;

            signin = "LogOut";
            manager = "none";
            userValid = "block";
            message = "";

            loadRequestRecord();
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

        private void loadRequestRecord()
        {
            record = [];

            try
            {
                database.Open();
                string query = "select dayoff, reason, subdate, apprv from time_off_request WHERE " +
                                "user_id = @UserID";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@UserID", user_id);

                MySqlDataReader reader = select.ExecuteReader();

                while (reader.Read())
                {
                    DateOnly dayoff = reader.GetDateOnly("dayoff");
                    DateOnly subdate = reader.GetDateOnly("subdate");
                    var newData = new Tuple<string, string, string, int>(dayoff.ToString(),
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

        private void RequestDayOff(DateOnly dayoff, DateOnly today, string reason)
        {
            if (user_id == 0) return;

            try
            {
                database.Open();
                string query = "INSERT INTO time_off_request (user_id, dayoff, reason, subdate) VALUES " +
                                "(@UserID, @DayOff, @Reason, @Subdate)";
                MySqlCommand insert = new MySqlCommand(query, database);
                insert.Parameters.AddWithValue("@UserID", user_id);
                insert.Parameters.AddWithValue("@DayOff", dayoff);
                insert.Parameters.AddWithValue("@Reason", reason);
                insert.Parameters.AddWithValue("@Subdate", today);
                insert.ExecuteNonQuery();

                message = "New Request Sent! Check updated Status regularly";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
                message = "This request cannot be submitted";
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
            UpdateNotify();

            // try sent user back to index if he tries coming back here without login
            if (user_id == 0)
                return RedirectToPage("Index");

            if (manager_id > 0)
                manager = "block";

            loadRequestRecord();

            return Page();
        }

        public void OnPost()
        {
            try
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
            }

            UpdateNotify();

            // Get current date
            DateTime now = DateTime.Today;
            DateOnly today = new DateOnly(now.Year, now.Month, now.Day);

            string absence = Request.Form["dayoff"];
            DateOnly dayoff = DateOnly.Parse(absence);

            string reason = Request.Form["reason"];

            // record new request
            RequestDayOff(dayoff, today, reason);

            OnGet();
        }
    }
}
