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
using System.Runtime.CompilerServices;
using System.Reflection;

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
        public int notify { get; set; }
        public int tot_request {  get; set; }

        public List<Tuple<string, DateOnly, string, int, int>> record { get; set; }
        public string message;

        public Schedule_AdjustmentModel(ILogger<Schedule_AdjustmentModel> logger)
        {
            database = new MySqlConnection(db_config);
            _logger = logger;

            signin = "LogOut";
            manager = "block";
            userValid = "block";
            message = "";

            tot_request = 0;
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

        private int FindWeekID(DateTime monday)
        {
            int week_id = 0;

            try
            {
                // Open the connection
                database.Open();
                string query = "select week_id from weekframe where start_day = @weekStart";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@weekStart", monday);

                object result = select.ExecuteScalar();

                if (result != null)
                {
                    week_id = Convert.ToInt32(result);
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
            return week_id;
        }

        private void loadAllRequestRecord()
        {
            record = [];

            try
            {
                database.Open();
                string query = "select user_id, first_name, last_name, dayoff, reason from time_off_request " +
                                "NATURAL INNER JOIN staff WHERE apprv = 0";
                MySqlCommand select = new MySqlCommand(query, database);

                MySqlDataReader reader = select.ExecuteReader();

                int index = 0;
                while (reader.Read())
                {
                    string username = reader.GetString("first_name") + " " + reader.GetString("last_name");

                    //DateOnly dayoff = reader.GetDateOnly("dayoff");
                    //DateOnly subdate = reader.GetDateOnly("subdate");
                    var newData = new Tuple<string, DateOnly, string, int, int>(username, reader.GetDateOnly("dayoff"),
                                   reader.GetString("reason"), reader.GetInt32("user_id"), index);
                    record.Add(newData);
                    index++;
                }

                tot_request = index;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private void ReviewRequest(int index, int decide)
        {
            if (index < 0) return;
            // STEP 1: get corresponding user id from the list
            int employee = record[index].Item4;
            DateOnly dayoff = record[index].Item2;

            // STEP 2: find the corresponding week
            string sdate = dayoff.ToString("yyyy-MM-dd");
            DateTime weekdate = DateTime.Parse(sdate);

            int currday = (int)weekdate.DayOfWeek;

            int daysSinceMonday = ((int)DayOfWeek.Monday - currday) % 7;
            if (daysSinceMonday > 0) { daysSinceMonday -= 7; }

            // compute each start day for 4 weeks
            DateTime monday = weekdate.AddDays(daysSinceMonday);

            int week_id = FindWeekID(monday);

            if (week_id == 0 && decide == 2) 
            { 
                message = "No Schedule Generated Yet for week of " + monday;
                return;
            }

            string day;

            if (currday == 1) day = "mon";
            else if (currday == 2) day = "tue";
            else if (currday == 3) day = "wed";
            else if (currday == 4) day = "thu";
            else if (currday == 5) day = "fri";
            else if (currday == 6) day = "sat";
            else  day = "sun";

            // STEP 3: Process request
            try
            {
                database.Open();

                string query = "UPDATE time_off_request SET apprv = @Veto " +
                                "WHERE user_id = @UserID and dayoff = @Day";
                MySqlCommand update = new MySqlCommand(query, database);
                update.Parameters.AddWithValue("@Veto", decide);
                update.Parameters.AddWithValue("@UserID", employee);
                update.Parameters.AddWithValue("@Day", dayoff);
                update.ExecuteNonQuery();

                if (decide == 1)
                    message = "Request Successfully Denied!";

                else if (decide == 2)
                {
                    query = "UPDATE assignment SET " + day + "_date = @OFF, " +
                                day + "_time = @OFF WHERE user_id = @UserID and week_id = @WeekID";
                    update = new MySqlCommand(query, database);
                    update.Parameters.AddWithValue("@OFF", DBNull.Value);
                    update.Parameters.AddWithValue("UserID", employee);
                    update.Parameters.AddWithValue("WeekID", week_id);
                    update.ExecuteNonQuery();

                    message = "Employee Schedule Updated!";
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

            UpdateNotify();

            // try sent user back to index if he tries coming back here without login
            if (user_id == 0)
                return RedirectToPage("Index");

            if (manager_id > 0)
                manager = "block";

            loadAllRequestRecord();

            return Page();
        }

        public void OnPost()
        {
            int index = -1;

            // reload the lists
            loadAllRequestRecord();

            if (Request.Form.ContainsKey("OK"))
            {
                index = int.Parse(Request.Form["OK"]);
                ReviewRequest(index, 2);
            }
            
            else if (Request.Form.ContainsKey("NO"))
            {
                index = int.Parse(Request.Form["NO"]);
                ReviewRequest(index, 1);
            }

            OnGet();
        }
    }
}
