using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Automatic_Scheduling_App.Models;
using MySqlConnector;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Linq;
namespace Automatic_Scheduling_App.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // HttpContext.Session.SetInt32("user_id", 7);
        }

        public IActionResult OnPost()
        {

            return RedirectToPage();
        }
    }
}
