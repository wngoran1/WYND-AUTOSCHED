namespace Automatic_Scheduling_App.Models
{
    public class BusinessNeeds
    {
        private string connectionString = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        //dept name variable
        public string DepartmentName { get; set; }

        //List of times, Day, (start, end), #) i.e. (Mon, (9-5), 4) means they need 4/(total monday people) 9-5 shifts on Mondays.
        public List<(string day, (TimeSpan startTime, TimeSpan endTime) timeframe, int amt)> Times { get; set; }

        //List of assignments needed i.e. (MTWUFXX, 4) means we need 4 people working mon, tue, weds, thurs, fri blocks (no sat sun)
        // (M T W U F A S) mon tue wed thu fri sat sun
        public List<(string assignment, int count)> Assignments { get; set; }

        //constructor for just business name
        public BusinessNeeds(string departmentName, Boolean fromDatabase)
        {
            DepartmentName = departmentName;
            Times = new List<(string, (TimeSpan, TimeSpan), int)>();
            Assignments = new List<(string, int)>();
            if (fromDatabase)
            {

            }
        }

        //constructor for everything. idrk if we're gonna need this though but in case.
        public BusinessNeeds(string departmentName, List<(string, int)> assignments, List<(string day, (TimeSpan startTime, TimeSpan endTime) timeframe, int amt)> times)
        {
            DepartmentName = departmentName;
            Assignments = new List<(string, int)>(assignments);
            Times = new List<(string, (TimeSpan, TimeSpan), int)>(times);
        }

        //Times contains all of the required times for each day. Defined by manager and interpreted as ratios.
        public void AddTime(string day, TimeSpan startTime, TimeSpan endTime, int number)
        {
            Times.Add((day, (startTime, endTime), number));
        }

        // Method to add an assignment
        public void AddAssignment(string assignment, int count)
        {
            Assignments.Add((assignment, count));
        }

        // Method to remove an assignment
        public void RemoveAssignment(string assignment)
        {
            var assignmentToRemove = Assignments.FirstOrDefault(a => a.assignment == assignment);
            if (assignmentToRemove != default)
            {
                Assignments.Remove(assignmentToRemove);
            }
        }

        // Method to alter an assignment
        public void AlterAssignment(string oldAssignment, string newAssignment, int newCount)
        {
            var index = Assignments.FindIndex(a => a.assignment == oldAssignment);
            if (index != -1)
            {
                Assignments[index] = (newAssignment, newCount);
            }
        }
        public void OverrideDatabase()
        {

        }
        public void UploadToDatabase()
        {

        }
    }
}
