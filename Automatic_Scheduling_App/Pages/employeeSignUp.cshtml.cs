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
    public class employeeSignUpModel : PageModel
    {
        private readonly ILogger<employeeSignUpModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        public string manager { get; set; }
        public string signin { get; set; }

        public employeeSignUpModel(ILogger<employeeSignUpModel> logger)
        {
            manager = "none";
            signin = "WYND Solutions";
            _logger = logger;
            database = new MySqlConnection(db_config);
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }
}
