using System.Linq.Expressions;

namespace Automatic_Scheduling_App.Models
{
    public class Availability
    {
        public string EmployeeID { get; set; }
        public string DeptName { get; set; }

        public Dictionary<DayOfWeek, (TimeSpan StartTime, TimeSpan EndTime)> DayAvailability { get; set; }

        public Availability(string employeeID, string deptName)
        {
            EmployeeID = employeeID;
            DeptName = deptName;
            DayAvailability = new Dictionary<DayOfWeek, (TimeSpan StartTime, TimeSpan EndTime)>();

        }
    }
}
