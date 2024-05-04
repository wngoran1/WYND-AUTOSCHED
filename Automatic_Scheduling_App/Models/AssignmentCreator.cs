using Automatic_Scheduling_App.Models;
using Automatic_Scheduling_App.Pages;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Automatic_Scheduling_App.Models
{
    public class AssignmentCreator
    {
        // private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        // public List<Assignment> AssignmentList { get; set; }
        public MySqlConnection database { get; set; }

        public AssignmentCreator(string db_config)
        {
            database = new MySqlConnection(db_config);
        }

        public void ListByDay(int week_id, string day)
        {
            List<Tuple<string, TimeSpan, TimeSpan, string, string, string>> assignments = [];

            try
            {
                // Open the connection
                database.Open();

                if (day == "mon") Console.WriteLine("\n___________ MONDAY\n");
                else if (day == "tue") Console.WriteLine("\n___________ TUESDAY\n");
                else if (day == "wed") Console.WriteLine("\n___________ WEDNESDAY\n");
                else if (day == "thu") Console.WriteLine("\n___________ THURSDAY\n");
                else if (day == "fri") Console.WriteLine("\n___________ FRIDAY\n");
                else if (day == "sat") Console.WriteLine("\n___________ SATURDAY\n");
                else if (day == "sun") Console.WriteLine("\n___________ SUNDAY\n");
                else return;

                // SQL query to select pattern and ratio from the weekly business need table
                string query = "select dept_name, start_time, end_time, first_name, Last_name, position " +
                            "from assignment natural inner join staff natural inner join department, timeframe " +
                            "where assignment." + day + "_time = timeframe.timeframe_id and week_id = 3 " +
                            "and " + day + "_date is not null " +
                            "order by dept_name, start_time, first_name";

                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@WeekID", week_id);
                MySqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the tupples to the list
                    var newData = new Tuple<string, TimeSpan, TimeSpan, string, string, string>(reader.GetString("dept_name"),
                                    reader.GetTimeSpan("start_time"), reader.GetTimeSpan("end_time"), reader.GetString("first_name"),
                                    reader.GetString("last_name"), reader.GetString("position"));
                    assignments.Add(newData);

                    // string wholeLine = string.Join(", ", new string[reader.FieldCount].Select((_, i) => reader.GetValue(i)));
                    // Console.WriteLine(wholeLine);
                }


                Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-20} {4,-20} {5,-20}", "[-Department-]", "[-Start-]", "[-End-]", "[-First Name-]", "[-Last Name-]", "[-Position-]");
                Console.WriteLine();

                foreach (var entry in assignments)
                {
                    Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-20} {4,-20} {5,-20}", entry.Item1, 
                        entry.Item2, entry.Item3, 
                        entry.Item4, entry.Item5, entry.Item6);
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
        }

        public void printAssignments(int week_id)
        {
            DateTime start_day;
            List<Tuple<string, string, string, string>> assignments = [];

            try
            {

                // Open the connection
                database.Open();

                // SQL query to retrieve the start date of the week to display
                string get_date = "select start_day from weekframe " +
                        "where week_id = @WeekID ";
                MySqlCommand select = new MySqlCommand(get_date, database);
                select.Parameters.AddWithValue("@WeekID", week_id);
                start_day = (DateTime)select.ExecuteScalar();

                Console.WriteLine("\n___________ Schedules for WEEK OF " + start_day.ToString("MM/dd/yyyy") + " ___________");
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

            ListByDay(week_id, "mon");
            ListByDay(week_id, "tue");
            ListByDay(week_id, "wed");
            ListByDay(week_id, "thu");
            ListByDay(week_id, "fri");
            ListByDay(week_id, "sat");
            ListByDay(week_id, "sun");
        }

        private List<int> shuffleList(List<int> list)
        {
            List<int> shuffledList = new List<int>(list);
            Random rng = new Random();
            int n = shuffledList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }
            return shuffledList;
        }

        private List<int> Download_employees_from(string department)
        {
            // List to store user IDs
            List<int> employees = [];

            try
            {

                // Open the connection
                database.Open();

                // SQL query to select staff IDs from the staff table
                string query = "SELECT user_id FROM staff natural inner join department " +
                            "where dept_name = @DeptName";
                // query = query + department;
                // Console.WriteLine(query);
                MySqlCommand command = new MySqlCommand(query, database); 
                command.Parameters.AddWithValue("@DeptName", department);
                MySqlDataReader reader = command.ExecuteReader();


                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the staff_id to the list
                    employees.Add(Convert.ToInt32(reader["user_id"]));
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
            return employees;
        }

        private List<Tuple<double, string>> Download_dept_pattern(string department)
        {
            List<Tuple<double, string>> patterns = [];

            try
            {

                // Open the connection
                database.Open();

                // SQL query to select pattern and ratio from the weekly business need table
                string query = "SELECT ratio, pattern " +
                       "FROM weekly_business_need " +
                       "NATURAL INNER JOIN department " +
                       "NATURAL INNER JOIN schedule_type " +
                       "WHERE dept_name = @DeptName";
                // query = query + department;

                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@DeptName", department);

                MySqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the tupples to the list
                    // we only collect patterns where count is not 0
                    // meaning the manager wants these shifts
                    // the sum of ration is always 1. checked at input
                    var newPattern = new Tuple<double, string>(reader.GetDouble("ratio"), reader.GetString("pattern"));
                    if (newPattern.Item1 > 0) patterns.Add(newPattern);
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

            return patterns;
        }

        private int SetNewWeek(string week_name, DateOnly start_date)
        {
            int week_id = 0;
            // we first create the week then retrive the id generated by the DB
            try
            {
                // Open the connection
                database.Open();
                string query = "select week_id from weekframe where start_day = @weekStart";
                MySqlCommand select = new MySqlCommand(query, database);
                select.Parameters.AddWithValue("@weekStart", start_date);

                object result = select.ExecuteScalar();

                if (result != null)
                {
                    week_id = Convert.ToInt32(result);

                    string deletequery = "delete from assignment where week_id = @weekID";
                    MySqlCommand delete = new MySqlCommand(deletequery, database);
                    delete.Parameters.AddWithValue("@weekID", week_id);
                    delete.ExecuteNonQuery();
                }

                else 
                { 
                    // SQL query to insert a new week tuple
                    string setQ = "INSERT INTO weekframe (week_name, start_day) " +
                            "VALUES (@WeekName, @WeekDay)";
                    MySqlCommand insert = new MySqlCommand(setQ, database);
                    insert.Parameters.AddWithValue("@WeekName", week_name);
                    insert.Parameters.AddWithValue("@WeekDay", start_date);
                    insert.ExecuteNonQuery();

                    /* SQL query to retrieve the id of the last week created above
                    string getQ = "select week_id from weekframe " +
                            "where week_name = @WeekName order by week_id DESC";
                    MySqlCommand retrieve = new MySqlCommand(getQ, database);
                    retrieve.Parameters.AddWithValue("@WeekName", week_name);*/

                    week_id = (int)select.ExecuteScalar();
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

        private void assign_schedule_to(int week_id, DateOnly start_date, string schedule, int employee)
        {
            if (schedule.Length < 7) return; // error in parsing the pattern

            try
            {
                // Open the connection
                database.Open();

                // SQL query to insert a new week tuple
                string query = "INSERT INTO assignment (week_id, user_id, mon_date, " +
                        "tue_date, wed_date, thu_date, " +
                        "fri_date, sat_date, sun_date) " +
                        "VALUES (@Week, @User, @Monday, " +
                        "@Tuesday, @Wednesday, @Thursday, " +
                        "@Friday, @Saturday, @Sunday)";
                MySqlCommand insert = new MySqlCommand(query, database);

                insert.Parameters.AddWithValue("@Week", week_id);
                insert.Parameters.AddWithValue("@User", employee);

                if (schedule[0] == 'M')
                    insert.Parameters.AddWithValue("@Monday", start_date);
                else
                    insert.Parameters.AddWithValue("@Monday", DBNull.Value);

                if (schedule[1] == 'T')
                    insert.Parameters.AddWithValue("@Tuesday", start_date.AddDays(1));
                else
                    insert.Parameters.AddWithValue("@Tuesday", DBNull.Value);

                if (schedule[2] == 'W')
                    insert.Parameters.AddWithValue("@Wednesday", start_date.AddDays(2));
                else
                    insert.Parameters.AddWithValue("@Wednesday", DBNull.Value);

                if (schedule[3] == 'U')
                    insert.Parameters.AddWithValue("@Thursday", start_date.AddDays(3));
                else
                    insert.Parameters.AddWithValue("@Thursday", DBNull.Value);

                if (schedule[4] == 'F')
                    insert.Parameters.AddWithValue("@Friday", start_date.AddDays(4));
                else
                    insert.Parameters.AddWithValue("@Friday", DBNull.Value);

                if (schedule[5] == 'A')
                    insert.Parameters.AddWithValue("@Saturday", start_date.AddDays(5));
                else
                    insert.Parameters.AddWithValue("@Saturday", DBNull.Value);

                if (schedule[6] == 'S')
                    insert.Parameters.AddWithValue("@Sunday", start_date.AddDays(6));
                else
                    insert.Parameters.AddWithValue("@Sunday", DBNull.Value);

                insert.ExecuteNonQuery();

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

        }
        
        public void GenerateAssignments(int week_id, string department, DateOnly start_date)
        {
            // This function create the schedules for the week based
            // on the patterns set beforehend for the given department

            // STEP 1: determine number of Employee in the department
            List<int> employees = Download_employees_from(department);
            employees = shuffleList(employees);

            // STEP 2: determine number of Employee in the department
            int numEmployee = employees.Count;
            if (numEmployee <= 0) return; // no need to process empty list

  

            // STEP 3: set interator to first employee in the list
            // determine procedure to shuffle the list (Optional for now)
            int index = 0;

            // STEP 4: retrieve the list of patterns for the department
            // for now we will use what has been set for the customer service
            // department and apply to all, it should be 0.5 for 2 patterns
            List<Tuple<double, string>> raw_data = Download_dept_pattern(department);
            List<Tuple<int, string>> patterns = [];

            // we convert the ratio into integer using the number of employee
            foreach (var entry in raw_data)
            {
                var tuple = new Tuple<int, string>((int)(entry.Item1 * numEmployee), entry.Item2);
                patterns.Add(tuple);
            }

            // now we compensate erros in count if anything happens
            // by adding the difference in number to first schedule
            int actual = patterns.Sum(tuple => tuple.Item1);
            if (numEmployee > actual)
            {
                var tuple = new Tuple<int, string>(numEmployee - actual, patterns[0].Item2);
                patterns.Add(tuple);
            }

            // STEP 5: assign the scheduled days

            while (index < numEmployee)
            {
                foreach (var schedule in patterns)
                {
                    for (int assigned = 0; assigned < schedule.Item1; assigned++)
                    {
                        assign_schedule_to(week_id, start_date, schedule.Item2, employees[index]);
                        index++;
                    }
                }
            }
        }

        private List<Tuple<double, int>> Download_daily_dept_timeframe(string department, string weekday)
        {
            List<Tuple<double, int>> timeframes = [];

            try
            {
                // Open the connection
                database.Open();

                // SQL query to select pattern and ratio from the weekly business need table
                string query = "SELECT ratio, timeframe_id " +
                       "FROM daily_business_need " +
                       "NATURAL INNER JOIN department " +
                       "WHERE dept_name = @DeptName and weekday = @WeekDay";

                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@DeptName", department);
                command.Parameters.AddWithValue("@WeekDay", weekday);

                MySqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the tupples to the list
                    // we only collect patterns where count is not 0
                    // the sum of ration is always 1. checked at input
                    var timeframe = new Tuple<double, int>(reader.GetDouble("ratio"), Convert.ToInt32(reader["timeframe_id"]));
                    if (timeframe.Item1 > 0) timeframes.Add(timeframe);
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

            return timeframes;
        }

        private List<int> Download_daily_assignment_from(int week_id, string department, string weekday)
        {
            List<int> assignments = [];

            try
            {
                // Open the connection
                database.Open();


                // SQL query to select assignment for a specific day of work
                string query = "select user_id from assignment " +
                            "natural inner join staff " +
                            "natural inner join department " +
                            "where week_id = @WeekID and " +
                            "dept_name = @DeptName and " +
                            weekday + "_date is not null ";

                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@WeekID", week_id);
                command.Parameters.AddWithValue("@DeptName", department);
                MySqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    assignments.Add(Convert.ToInt32(reader["user_id"]));
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

            return assignments;
        }

        private void update_assignment_of(int week_id, string weekday, int timeframe, int employee)
        {
            try
            {
                // Open the connection
                database.Open();

                // SQL query to insert a new week tuple
                string query = "update assignment " +
                        "set " + weekday + "_time = @Time " +
                        "where week_id = @WeekID and user_id = @User ";

                MySqlCommand insert = new MySqlCommand(query, database);

                insert.Parameters.AddWithValue("@Time", timeframe);
                insert.Parameters.AddWithValue("@WeekID", week_id);
                insert.Parameters.AddWithValue("@User", employee);

                insert.ExecuteNonQuery();

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
        
        }

        private void AssignDailyTimes(int week_id, string department, string weekday)
        {
            // This function assign the time period for each day

            // STEP 1: retrive the list of Employees in the department working on the specified day
            List<int> assignments = Download_daily_assignment_from(week_id, department, weekday);
            assignments = shuffleList(assignments);

            // STEP 2: determine number of Employee in the department
            int numAssignment = assignments.Count;
            if (numAssignment <= 0) return; // no need to process empty list

            // STEP 3: set interator to first assignment in the list
            // determine procedure to shuffle the list (Optional for now)
            int index = 0;

            // STEP 4: retrieve the list of timeframe for the department
            List<Tuple<double, int>> raw_data = Download_daily_dept_timeframe(department, weekday);

            List<Tuple<int, int>> timeset = [];

            // we convert the ratio into integer using the number of employee

            foreach (var entry in raw_data)
            {
                var tuple = new Tuple<int, int>((int)(entry.Item1 * numAssignment), entry.Item2);
                timeset.Add(tuple);
            }

            // now we compensate erros in count if anything happens
            // by adding the difference in number to first schedule
            int actual = timeset.Sum(tuple => tuple.Item1);
            if (numAssignment > actual)
            {
                var tuple = new Tuple<int, int>(numAssignment - actual, timeset[0].Item2);
                timeset.Add(tuple);
            }

            // STEP 5: assign the timeframe days

            foreach (var timeframe in timeset)
            {
                for (int assigned = 0; assigned < timeframe.Item1; assigned++)
                {
                    update_assignment_of(week_id, weekday, timeframe.Item2, assignments[index]);
                    index++;
                }
            }
        }

        private void GenerateTimes(int week_id, string department)
        {
            // This function assign the time period based on business need
            // for a specified department
            string[] weekdays = { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            foreach (string weekday in weekdays) 
            {
                AssignDailyTimes(week_id, department, weekday); 
            }
        }

        public void AutoScheduler(string week_name, DateOnly start_date)
        {
            // This function create the schedules for the week based
            // on the patterns set beforehend for the given department

            // NOTE: for this test we make the assignment globally for everyone
            // later we will proceed by department

            // STEP 0: setup the week info
            int week_id = SetNewWeek(week_name, start_date);

            // collect the list of department
            List<string> departments = [];

            try
            {
                database.Open();
                // here we only select the departments that have employees
                string query = "select distinct dept_name from department natural inner join staff";

                MySqlCommand command = new MySqlCommand(query, database);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Retrieve department name from the row
                    departments.Add(reader.GetString("dept_name"));
                }

                reader.Close();
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
            foreach (string department in departments) 
            {
                // create the schedule for each department
                GenerateAssignments(week_id, department, start_date);
                GenerateTimes(week_id, department);
                // Console.WriteLine(department);
            }

            // printAssignments(week_id);

        }

        public void UploadToDB()
        {
            // define here
        }
    }
}