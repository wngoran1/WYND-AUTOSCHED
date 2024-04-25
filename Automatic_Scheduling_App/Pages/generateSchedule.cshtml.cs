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

namespace Automatic_Scheduling_App.Pages
{
    public class generateScheduleModel : PageModel
    {
        private readonly ILogger<generateScheduleModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";

        //temporary variable while debuging assignment creation
        public string message { get; set; }

        public bool progress { get; set; }
        private MySqlConnection database { get; set; }
        public string signin { get; set; }
        public string manager { get; set; }

        public generateScheduleModel(ILogger<generateScheduleModel> logger)
        {
            _logger = logger;
            signin = "LogOut";
            manager = "block";
        }
        public void OnGet()
        {
            progress = true;
            message = "";
        }
        public void OnPost()
        {
            progress = true;
            // collect week start dates for current and next
            DateTime today = DateTime.Today;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek) % 7;
            DateTime monday = today.AddDays(daysUntilMonday);
            DateTime nextweek = monday.AddDays(7);

            //test stuff for creating some schedules, will elete)
            DateOnly start_date = new DateOnly(monday.Year, monday.Month, monday.Day);
            string week_name = "Week of " + start_date.ToString();

            DateOnly start_date_2 = new DateOnly(nextweek.Year, nextweek.Month, nextweek.Day);
            string week_name_2 = "Week of " + start_date_2.ToString();

            AssignmentCreator newAssignment = new AssignmentCreator(db_config);

            newAssignment.AutoScheduler(week_name, start_date);
            newAssignment.AutoScheduler(week_name_2, start_date_2); 

            message = "###  New schedules have been generated  ###";

            DateTime dateTime = DateTime.Now; // Example DateTime object
            DateTime dateOnly = dateTime.Date;
        }

    }
}
