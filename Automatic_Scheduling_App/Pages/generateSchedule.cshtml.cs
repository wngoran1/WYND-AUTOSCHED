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
using System.Diagnostics;
using Microsoft.AspNetCore.Html;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Automatic_Scheduling_App.Pages
{
    public class generateScheduleModel : PageModel
    {
        private readonly ILogger<generateScheduleModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";

        //temporary variable while debuging assignment creation
        public string message { get; set; }
        private MySqlConnection database { get; set; }
        public string signin { get; set; }
        public string manager { get; set; }
        public string loading { get; set; }
        public string prevDis { get; set; }
        public string nextDis { get; set; }
        private int display_week;
        public string week_name { get; set; }
        public string week_day { get; set; }
        public List<string> weeks { get; set; }

        public List<Tuple<string, TimeSpan, TimeSpan, string, string>> week_list { get; set; }
        private int index;
        private DateOnly mon_w1;
        private DateOnly mon_w2;
        private DateOnly mon_w3;
        private DateOnly mon_w4;

        public generateScheduleModel(ILogger<generateScheduleModel> logger)
        {
            database = new MySqlConnection(db_config);

            loading = "none";
            _logger = logger;
            signin = "LogOut";
            manager = "block";
            display_week = 0;

            loadWeekDates();

            week_name = weeks[0];
            week_day = "Unknown";
            message = "";
            prevDis = "";
            nextDis = "";
        }

        private void loadWeekDates()
        {
            weeks = [];

            // find monday date of current week
            DateTime today = DateTime.Today;

            int daysSinceMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek) % 7;
            if (daysSinceMonday > 0) { daysSinceMonday -= 7; }

            // compute each start day for 4 weeks
            DateTime monday = today.AddDays(daysSinceMonday);
            DateTime nextweek = monday.AddDays(7);
            DateTime thirdweek = monday.AddDays(14);
            DateTime fourthweek = monday.AddDays(21);

            // set each week start day
            mon_w1 = new DateOnly(monday.Year, monday.Month, monday.Day);
            mon_w2 = new DateOnly(nextweek.Year, nextweek.Month, nextweek.Day);
            mon_w3 = new DateOnly(thirdweek.Year, thirdweek.Month, thirdweek.Day);
            mon_w4 = new DateOnly(fourthweek.Year, fourthweek.Month, fourthweek.Day);

            // set Week names
            weeks.Add("Week of " + mon_w1.ToString());
            weeks.Add("Week of " + mon_w2.ToString());
            weeks.Add("Week of " + mon_w3.ToString());
            weeks.Add("Week of " + mon_w4.ToString());
        }

        private int SetNewWeek(DateOnly start_date)
        {
            int week_id = 0;
            // we first create the week then retrive the id generated by the DB
            try
            {
                // Open the connection
                database.Open();
                string sQuery = "select week_id from weekframe where start_day = @weekStart";
                MySqlCommand select = new MySqlCommand(sQuery, database);
                select.Parameters.AddWithValue("@weekStart", start_date);

                object result = select.ExecuteScalar();
                if (result != null)
                    week_id = Convert.ToInt32(result);
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

        public void ListByDay(int week_id, string day)
        {
            List<Tuple<string, TimeSpan, TimeSpan, string, string>> assignments = [];

            try
            {
                // Open the connection
                database.Open();

                // SQL query to select the assignments
                string rQuery = "select dept_name, start_time, end_time, first_name, Last_name, position " +
                        "from assignment natural inner join staff natural inner join department, timeframe " +
                        "where assignment." + day + "_time = timeframe.timeframe_id and week_id = 3 " +
                        "and " + day + "_date is not null and ssn is not null " +
                        "order by dept_name, start_time, first_name";

                MySqlCommand command = new MySqlCommand(rQuery, database);
                command.Parameters.AddWithValue("@WeekID", week_id);
                MySqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the tupples to the list
                    string username = reader.GetString("first_name") + " " + reader.GetString("last_name");
                    var newData = new Tuple<string, TimeSpan, TimeSpan, string, string>(reader.GetString("dept_name"),
                                    reader.GetTimeSpan("start_time"), reader.GetTimeSpan("end_time"), 
                                    username, reader.GetString("position"));
                    assignments.Add(newData);

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

            week_list = assignments;

            if (day == "mon") week_day = "Monday";
            else if (day == "tue") week_day = "Tuesday";
            else if (day == "wed") week_day = "Wednesday";
            else if (day == "thu") week_day = "Thursday";
            else if (day == "fri") week_day = "Friday";
            else if (day == "sat") week_day = "Saturday";
            else if (day == "sun") week_day = "Sunday";

        }

        public void OnGet()
        {
            // when lending on page first time
            nextDis = "";
            prevDis = "disabled";
            index = 1;
            HttpContext.Session.SetInt32("index", index);

            display_week = SetNewWeek(mon_w1);

            if (display_week == 0)
            {
                // check if week exists
                week_list = new List<Tuple<string, TimeSpan, TimeSpan, string, string>>();
            }
            else
            {
                // display monday
                ListByDay(display_week, "mon");
            }

            
        }
        public void OnPost()
        {

            // newAssignment.AutoScheduler(week_name, start_date);

            // newAssignment.AutoScheduler(week_name_2, start_date_2);

            // newAssignment.AutoScheduler(week_name_3, start_date_3);

            try
            {
                index = (int)HttpContext.Session.GetInt32("index");
            }
            catch (Exception ex) 
            {
                index = 2;
            }
            week_day = "mon";
            

            if (Request.Form.ContainsKey("prevbut"))
            {
                index--;
                // update stored value
                HttpContext.Session.SetInt32("index", index);
            }
            else if (Request.Form.ContainsKey("nextbut"))
            {
                index++;
                // update stored value
                HttpContext.Session.SetInt32("index", index);
            }
            else if (Request.Form.ContainsKey("reset_all"))
            {
                // make all the assignments
                AssignmentCreator newAssignment = new AssignmentCreator(db_config);
                newAssignment.AutoScheduler(week_name, mon_w1);
                newAssignment.AutoScheduler(week_name, mon_w2);
                newAssignment.AutoScheduler(week_name, mon_w3);
                newAssignment.AutoScheduler(week_name, mon_w4);

                message = "###  New schedules have been generated  ###";
            }
            else if (Request.Form.ContainsKey("reset_week"))
            {
                AssignmentCreator newAssignment = new AssignmentCreator(db_config);
                // make the assignment for corresponding week
                if (index == 1)
                    newAssignment.AutoScheduler(week_name, mon_w1);
                else if (index == 2)
                    newAssignment.AutoScheduler(week_name, mon_w2);
                else if (index == 3)
                    newAssignment.AutoScheduler(week_name, mon_w3);
                else if (index == 4)
                    newAssignment.AutoScheduler(week_name, mon_w4);

                message = "###  New schedules have been generated  ###";
            }
            else if (Request.Form.ContainsKey("reset_day"))
            {
                week_day = Request.Form["week_day"];
            }

            week_name = weeks[index - 1];

            if (index == 1)
                display_week = SetNewWeek(mon_w1);
            else if (index == 2)
                display_week = SetNewWeek(mon_w2);
            else if (index == 3)
                display_week = SetNewWeek(mon_w3);
            else if (index == 4)
                display_week = SetNewWeek(mon_w4);
            else
                display_week = 0;

            if (display_week == 0)
            {
                week_list = new List<Tuple<string, TimeSpan, TimeSpan, string, string>>();
            }
            else
            {
                Console.WriteLine(week_day);
                // display each day
                ListByDay(display_week, week_day);
            }

            // set navigation flags
            if (index < 2)
            {
                nextDis = "";
                prevDis = "disabled";
            }
            if (index > 3)
            {
                nextDis = "disabled";
                prevDis = "";
            }

        }

    }
}
