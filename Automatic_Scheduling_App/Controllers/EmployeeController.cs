using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Automatic_Scheduling_App.Controllers
{
    public class EmployeeController : Controller
    {
        [HttpPost]
        public IActionResult GetInfo(int employeeID)
        {
            return PartialView("_EmployeeSchedule");
        }
    }
}