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
    public class dashboardModel : PageModel
    {
        private readonly ILogger<dashboardModel> _logger;

        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        public string manager { get; set; }
        public int manager_id;
        private int user_id;
        public string userValid { get; set; }
        public string signin { get; set; }
        public int notify { get; set; }

        public dashboardModel(ILogger<dashboardModel> logger)
        {
            _logger = logger;
            database = new MySqlConnection(db_config);
            signin = "LogOut";
            userValid = "block";
            manager = "none";
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
        //User Data variables
        public string userName { get; set; }
        public string user_dept { get; set; }
        public string user_role { get; set; }

        // current week data variables
        public List<int> currweek_days { get; set; }
        public List<string> currweek_stime { get; set; }
        public List<string> currweek_etime { get; set; }

        // next week data variables
        public List<int> nextweek_days { get; set; }
        public List<string> nextweek_stime { get; set; }
        public List<string> nextweek_etime { get; set; }

        // third week data variables
        public List<int> thirdweek_days { get; set; }
        public List<string> thirdweek_stime { get; set; }
        public List<string> thirdweek_etime { get; set; }

        // set distinct color for current day
        public string monbg = DateTime.Today.DayOfWeek == DayOfWeek.Monday ? "lightcyan" : "white";
        public string tuebg = DateTime.Today.DayOfWeek == DayOfWeek.Tuesday ? "lightcyan" : "white";
        public string wedbg = DateTime.Today.DayOfWeek == DayOfWeek.Wednesday ? "lightcyan" : "white";
        public string thubg = DateTime.Today.DayOfWeek == DayOfWeek.Thursday ? "lightcyan" : "white";
        public string fribg = DateTime.Today.DayOfWeek == DayOfWeek.Friday ? "lightcyan" : "white";
        public string satbg = DateTime.Today.DayOfWeek == DayOfWeek.Saturday ? "lightcyan" : "white";
        public string sunbg = DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? "lightcyan" : "white";

        private void SetUserData()
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
                    user_dept = reader.GetString("dept_name");
                    user_role = reader.GetString("position");
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
                userName = user[0] + " " + user[1];
            }
            else
            {
                userName = "Unknown USER";
                user_dept = "Unknown Department";
                user_role = "Unknown Role";
            }
        }

        private int Download_week_id(string start_date)
        {
            int week_id = -1;
            try
            {
                database.Open();
                string query = "select week_id from weekframe where start_day = @weekStart";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@weekStart", start_date);

                MySqlDataReader reader = select.ExecuteReader();

                // Iterate through the result set
                if (reader.Read())
                {
                    week_id = Convert.ToInt32(reader["week_id"]);
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
           
            return week_id;
        }

        private void SetCalendarDays()
        {
            // collect week start dates for current and next
            DateTime today = DateTime.Today;

            int daysSinceMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek) % 7;
            if (daysSinceMonday > 0) { daysSinceMonday -= 7; }

            DateTime monday = today.AddDays(daysSinceMonday); 
            DateTime nextweek = monday.AddDays(7);
            DateTime thirdweek = monday.AddDays(14);

            // retrive respective week ids from database
            int currweek_id = Download_week_id(monday.ToString("yyyy-MM-dd"));
            int nextweek_id = Download_week_id(nextweek.ToString("yyyy-MM-dd"));
            int thirdweek_id = Download_week_id(thirdweek.ToString("yyyy-MM-dd"));

            // update the calendar month days
            for (int days = 0; days < 7; days++)
            {
                currweek_days[days] = monday.AddDays(days).Day;
                nextweek_days[days] = nextweek.AddDays(days).Day;
                thirdweek_days[days] = thirdweek.AddDays(days).Day;
            }

            // download time data for the employee
            string[] weekdays = { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };

            foreach (string weekday in weekdays)
            {
                if (currweek_id > 0)
                    Download_employee_schedule_for(1, currweek_id, weekday);

                if (nextweek_id > 0)
                    Download_employee_schedule_for(2, nextweek_id, weekday);

                if (thirdweek_id > 0)
                    Download_employee_schedule_for(3, thirdweek_id, weekday);
            }
        }

        private void Download_employee_schedule_for(int currweek, int week_id, string day)
        {
            int currday = -1;

            // set index for day traversal
            if (day == "mon") currday = 0;
            else if (day == "tue") currday = 1;
            else if (day == "wed") currday = 2;
            else if (day == "thu") currday = 3;
            else if (day == "fri") currday = 4;
            else if (day == "sat") currday = 5;
            else if (day == "sun") currday = 6;


            try
            {
                database.Open();

                string query = "select start_time, end_time from assignment, timeframe where week_id = @weekID and user_id = @userID and " +
                               day + "_time = timeframe_id";

                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@weekID", week_id);
                select.Parameters.AddWithValue("@userID", user_id);

                MySqlDataReader reader = select.ExecuteReader();


                // Iterate through the result set
                while (reader.Read())
                {  
                    if (currweek == 1)
                    {
                        currweek_stime[currday] = reader.GetTimeSpan("start_time").ToString("hh\\:mm");
                        currweek_etime[currday] = reader.GetTimeSpan("end_time").ToString("hh\\:mm");
                    }
                    else if (currweek == 2)
                    {
                        nextweek_stime[currday] = reader.GetTimeSpan("start_time").ToString("hh\\:mm");
                        nextweek_etime[currday] = reader.GetTimeSpan("end_time").ToString("hh\\:mm");
                    }
                    else if (currweek == 3)
                    {
                        thirdweek_stime[currday] = reader.GetTimeSpan("start_time").ToString("hh\\:mm");
                        thirdweek_etime[currday] = reader.GetTimeSpan("end_time").ToString("hh\\:mm");
                    }
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

        }

        public IActionResult OnGet()
        {
            // initialize startup variables
            // database = new MySqlConnection(db_config);

            // initialize current week data

            currweek_days = new List<int>(new int[7]);
            currweek_stime = new List<string> { "DAY", "DAY", "DAY", "DAY", "DAY", "DAY", "DAY" };
            currweek_etime = new List<string> { "OFF", "OFF", "OFF", "OFF", "OFF", "OFF", "OFF" };

            // initialize next week data
            nextweek_days = new List<int>(new int[7]);
            nextweek_stime = new List<string> { "DAY", "DAY", "DAY", "DAY", "DAY", "DAY", "DAY" };
            nextweek_etime = new List<string> { "OFF", "OFF", "OFF", "OFF", "OFF", "OFF", "OFF" };

            // initialize next week data
            thirdweek_days = new List<int>(new int[7]);
            thirdweek_stime = new List<string> { "DAY", "DAY", "DAY", "DAY", "DAY", "DAY", "DAY" };
            thirdweek_etime = new List<string> { "OFF", "OFF", "OFF", "OFF", "OFF", "OFF", "OFF" };

            try
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
                manager_id = (int)HttpContext.Session.GetInt32("manager_id");
            }
            catch (Exception ex) {
                user_id = 0;
                manager_id = 0;
            }
            UpdateNotify();

            // try sent user back to index if he tries coming back here without login
            if (user_id == 0)
                return RedirectToPage("Index");

            if (manager_id > 0)
                manager = "block";

            SetUserData();
            SetCalendarDays();

            return Page();

        }

        public IActionResult OnPost()
        {
            user_id = int.Parse(Request.Form["userId"]);
            HttpContext.Session.SetInt32("user_id", user_id);
            return RedirectToPage();
        }
    }
}