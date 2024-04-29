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

        public int complete {  get; set; }

        public generateScheduleModel(ILogger<generateScheduleModel> logger)
        {
            _logger = logger;
            signin = "LogOut";
            manager = "block";
        }

        public void OnGet()
        {
            message = "";
            complete = 0;
        }
        public void OnPost()
        {
            complete = 0;

            // collect week start dates for current and next
            DateTime today = DateTime.Today;

            int daysSinceMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek) % 7;
            if (daysSinceMonday > 0) { daysSinceMonday -= 7; }

            DateTime monday = today.AddDays(daysSinceMonday);
            DateTime nextweek = monday.AddDays(7);
            DateTime thirdweek = monday.AddDays(14);

            //test stuff for creating some schedules, will delete later)
            DateOnly start_date = new DateOnly(monday.Year, monday.Month, monday.Day);
            string week_name = "Week of " + start_date.ToString();

            DateOnly start_date_2 = new DateOnly(nextweek.Year, nextweek.Month, nextweek.Day);
            string week_name_2 = "Week of " + start_date_2.ToString();

            DateOnly start_date_3 = new DateOnly(thirdweek.Year, thirdweek.Month, thirdweek.Day);
            string week_name_3 = "Week of " + start_date_3.ToString();

            AssignmentCreator newAssignment = new AssignmentCreator(db_config);

            newAssignment.AutoScheduler(week_name, start_date);

            newAssignment.AutoScheduler(week_name_2, start_date_2);

            newAssignment.AutoScheduler(week_name_3, start_date_3);

            message = "###  New schedules have been generated  ###";

            DateTime dateTime = DateTime.Now; // Example DateTime object
            DateTime dateOnly = dateTime.Date;

            complete = 100;
        }

    }
}
