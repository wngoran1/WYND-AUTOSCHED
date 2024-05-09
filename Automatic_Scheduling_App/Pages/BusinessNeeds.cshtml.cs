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
    public class BusinessNeedsModel : PageModel
    {
        private readonly ILogger<BusinessNeedsModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";

        private MySqlConnection database;
        private int user_id;

        private List<Tuple<double, string>> patterns;
        private List<Tuple<double, string>> update_patterns;

        private List<Tuple<double, string, string>> update_timeframes;
        private List<Tuple<double, TimeSpan, TimeSpan>> mon_timeframes;
        private List<Tuple<double, TimeSpan, TimeSpan>> tue_timeframes;
        private List<Tuple<double, TimeSpan, TimeSpan>> wed_timeframes;
        private List<Tuple<double, TimeSpan, TimeSpan>> thu_timeframes;
        private List<Tuple<double, TimeSpan, TimeSpan>> fri_timeframes;
        private List<Tuple<double, TimeSpan, TimeSpan>> sat_timeframes;
        private List<Tuple<double, TimeSpan, TimeSpan>> sun_timeframes;

        public string signin { get; set; }
        public string manager { get; set; }
        public string userValid { get; set; }
        public string addtime { get; set; }
        public string addtime_but { get; set; }
        public string dept_name { get; set; }
        public int notify { get; set; }
        public int dept_id { get; set; }

        public List<string> weekP1 { get; set; }
        public int ratio_w1 { get; set; }
        public List<string> weekP2 { get; set; }
        public int ratio_w2 { get; set; }
        public List<string> weekP3 { get; set; }
        public int ratio_w3 { get; set; }
        public List<string> weekP4 { get; set; }
        public int ratio_w4 { get; set; }

        public List<int> monRatio { get; set; }
        public List<int> tueRatio { get; set; }
        public List<int> wedRatio { get; set; }
        public List<int> thuRatio { get; set; }
        public List<int> friRatio { get; set; }
        public List<int> satRatio { get; set; }
        public List<int> sunRatio { get; set; }

        public List<string> monTimes { get; set; }
        public List<string> tueTimes { get; set; }
        public List<string> wedTimes { get; set; }
        public List<string> thuTimes { get; set; }
        public List<string> friTimes { get; set; }
        public List<string> satTimes { get; set; }
        public List<string> sunTimes { get; set; }

        public Dictionary<int, string> departments { get; set; }
        public string message { get; set; }
        public string time_message { get; set; }


        public BusinessNeedsModel(ILogger<BusinessNeedsModel> logger)
        {
            _logger = logger;
            signin = "LogOut";
            manager = "block";
            userValid = "block";
            addtime = "none";
            addtime_but = "block";
            message = "";
            time_message = "";
            database = new MySqlConnection(db_config);

            loadDepartments();

            weekP1 = new List<string> { "", "", "", "", "", "", "" };
            weekP2 = new List<string> { "", "", "", "", "", "", "" };
            weekP3 = new List<string> { "", "", "", "", "", "", "" };
            weekP4 = new List<string> { "", "", "", "", "", "", "" };

            dept_name = "Unknown Department";
            dept_id = 0;
            user_id = 0;

            patterns = [];
            update_patterns = [];
            update_timeframes = [];

            mon_timeframes = [];
            tue_timeframes = [];
            wed_timeframes = [];
            thu_timeframes = [];
            fri_timeframes = [];
            sat_timeframes = [];
            sun_timeframes = [];

            ratio_w1 = 0;
            ratio_w2 = 0;
            ratio_w3 = 0;
            ratio_w4 = 0;

            monTimes = new List<string> { "", "", "", "", "", "" };
            tueTimes = new List<string> { "", "", "", "", "", "" };
            wedTimes = new List<string> { "", "", "", "", "", "" };
            thuTimes = new List<string> { "", "", "", "", "", "" };
            friTimes = new List<string> { "", "", "", "", "", "" };
            satTimes = new List<string> { "", "", "", "", "", "" };
            sunTimes = new List<string> { "", "", "", "", "", "" };

            monRatio = new List<int> { 0, 0, 0 };
            tueRatio = new List<int> { 0, 0, 0 };
            wedRatio = new List<int> { 0, 0, 0 };
            thuRatio = new List<int> { 0, 0, 0 };
            friRatio = new List<int> { 0, 0, 0 };
            satRatio = new List<int> { 0, 0, 0 };
            sunRatio = new List<int> { 0, 0, 0 };
            
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
        private void loadDepartments()
        {
            departments = new Dictionary<int, string>();
            try
            {
                database.Open();
                string query = "select dept_id, dept_name from department order by dept_name";
                MySqlCommand select = new MySqlCommand(query, database);

                MySqlDataReader reader = select.ExecuteReader();
                // Iterate through the result set
                while (reader.Read())
                {
                    // update the list of departments
                    departments[reader.GetInt32("dept_id")] = reader.GetString("dept_name");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions
            }
            finally { database.Close(); }
        }

        private void Download_dept_pattern()
        {
            patterns = [];

            if (dept_id == 0) return; // invalid department

            try
            {

                // Open the connection
                database.Open();

                // SQL query to select pattern and ratio from the weekly business need table
                string query = "SELECT ratio, pattern " +
                       "FROM weekly_business_need " +
                       "NATURAL INNER JOIN schedule_type " +
                       "WHERE dept_id = @DeptID";
                // query = query + department;

                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@DeptID", dept_id);

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
        }

        private List<Tuple<double, TimeSpan, TimeSpan>> Download_dept_timeframe(string day)
        {
            List<Tuple<double, TimeSpan, TimeSpan>> timeframes = [];

            try
            {

                // Open the connection
                database.Open();

                // SQL query to select pattern and ratio from the weekly business need table
                string query = "SELECT ratio, start_time, end_time " +
                       "FROM daily_business_need " +
                       "NATURAL INNER JOIN timeframe " +
                       "WHERE dept_id = @DeptID AND weekday= @Day";

                MySqlCommand command = new MySqlCommand(query, database);
                command.Parameters.AddWithValue("@DeptID", dept_id);
                command.Parameters.AddWithValue("@Day", day);
                MySqlDataReader reader = command.ExecuteReader();

                // Iterate through the result set
                while (reader.Read())
                {
                    // Retrieve and add the tupples to the list
                    // we only collect patterns where count is not 0
                    // meaning the manager wants these shifts
                    // the sum of ration is always 1. checked at input
                    var newPattern = new Tuple<double, TimeSpan, TimeSpan>(reader.GetDouble("ratio"), reader.GetTimeSpan("start_time"), reader.GetTimeSpan("end_time"));
                    if (newPattern.Item1 > 0) timeframes.Add(newPattern);
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

        private void Download_dept_times()
        {
            if (dept_id == 0) return; // invalid department

            mon_timeframes = Download_dept_timeframe("mon");
            tue_timeframes = Download_dept_timeframe("tue");
            wed_timeframes = Download_dept_timeframe("wed");
            thu_timeframes = Download_dept_timeframe("thu");
            fri_timeframes = Download_dept_timeframe("fri");
            sat_timeframes = Download_dept_timeframe("sat");
            sun_timeframes = Download_dept_timeframe("sun");
        }

        private void Display_dept_monTime()
        {
            if (dept_id == 0) return; // invalid department

            if (mon_timeframes.Count > 0)
            {
                monRatio[0] = (int)(mon_timeframes[0].Item1 * 100);
                monTimes[0] = mon_timeframes[0].Item2.ToString();
                monTimes[1] = mon_timeframes[0].Item3.ToString();
            }

            if (mon_timeframes.Count > 1)
            {
                monRatio[1] = (int)(mon_timeframes[1].Item1 * 100);
                monTimes[2] = mon_timeframes[1].Item2.ToString();
                monTimes[3] = mon_timeframes[1].Item3.ToString();
            }

            if (mon_timeframes.Count > 2)
            {
                monRatio[2] = (int)(mon_timeframes[2].Item1 * 100);
                monTimes[4] = mon_timeframes[2].Item2.ToString();
                monTimes[5] = mon_timeframes[2].Item3.ToString();
            }
        }

        private void Display_dept_tueTime()
        {
            if (dept_id == 0) return; // invalid department

            if (tue_timeframes.Count > 0)
            {
                tueRatio[0] = (int)(tue_timeframes[0].Item1 * 100);
                tueTimes[0] = tue_timeframes[0].Item2.ToString();
                tueTimes[1] = tue_timeframes[0].Item3.ToString();
            }

            if (tue_timeframes.Count > 1)
            {
                tueRatio[1] = (int)(tue_timeframes[1].Item1 * 100);
                tueTimes[2] = tue_timeframes[1].Item2.ToString();
                tueTimes[3] = tue_timeframes[1].Item3.ToString();
            }

            if (tue_timeframes.Count > 2)
            {
                tueRatio[2] = (int)(tue_timeframes[2].Item1 * 100);
                tueTimes[4] = tue_timeframes[2].Item2.ToString();
                tueTimes[5] = tue_timeframes[2].Item3.ToString();
            }
        }

        private void Display_dept_wedTime()
        {
            if (dept_id == 0) return; // invalid department

            if (wed_timeframes.Count > 0)
            {
                wedRatio[0] = (int)(wed_timeframes[0].Item1 * 100);
                wedTimes[0] = wed_timeframes[0].Item2.ToString();
                wedTimes[1] = wed_timeframes[0].Item3.ToString();
            }

            if (wed_timeframes.Count > 1)
            {
                wedRatio[1] = (int)(wed_timeframes[1].Item1 * 100);
                wedTimes[2] = wed_timeframes[1].Item2.ToString();
                wedTimes[3] = wed_timeframes[1].Item3.ToString();
            }

            if (wed_timeframes.Count > 2)
            {
                wedRatio[2] = (int)(wed_timeframes[2].Item1 * 100);
                wedTimes[4] = wed_timeframes[2].Item2.ToString();
                wedTimes[5] = wed_timeframes[2].Item3.ToString();
            }
        }

        private void Display_dept_thuTime()
        {
            if (dept_id == 0) return; // invalid department

            if (thu_timeframes.Count > 0)
            {
                thuRatio[0] = (int)(thu_timeframes[0].Item1 * 100);
                thuTimes[0] = thu_timeframes[0].Item2.ToString();
                thuTimes[1] = thu_timeframes[0].Item3.ToString();
            }

            if (thu_timeframes.Count > 1)
            {
                thuRatio[1] = (int)(thu_timeframes[1].Item1 * 100);
                thuTimes[2] = thu_timeframes[1].Item2.ToString();
                thuTimes[3] = thu_timeframes[1].Item3.ToString();
            }

            if (thu_timeframes.Count > 2)
            {
                thuRatio[2] = (int)(thu_timeframes[2].Item1 * 100);
                thuTimes[4] = thu_timeframes[2].Item2.ToString();
                thuTimes[5] = thu_timeframes[2].Item3.ToString();
            }
        }

        private void Display_dept_friTime()
        {
            if (dept_id == 0) return; // invalid department

            if (fri_timeframes.Count > 0)
            {
                friRatio[0] = (int)(fri_timeframes[0].Item1 * 100);
                friTimes[0] = fri_timeframes[0].Item2.ToString();
                friTimes[1] = fri_timeframes[0].Item3.ToString();
            }

            if (fri_timeframes.Count > 1)
            {
                friRatio[1] = (int)(fri_timeframes[1].Item1 * 100);
                friTimes[2] = fri_timeframes[1].Item2.ToString();
                friTimes[3] = fri_timeframes[1].Item3.ToString();
            }

            if (fri_timeframes.Count > 2)
            {
                friRatio[2] = (int)(fri_timeframes[2].Item1 * 100);
                friTimes[4] = fri_timeframes[2].Item2.ToString();
                friTimes[5] = fri_timeframes[2].Item3.ToString();
            }
        }

        private void Display_dept_satTime()
        {
            if (dept_id == 0) return; // invalid department

            if (sat_timeframes.Count > 0)
            {
                satRatio[0] = (int)(sat_timeframes[0].Item1 * 100);
                satTimes[0] = sat_timeframes[0].Item2.ToString();
                satTimes[1] = sat_timeframes[0].Item3.ToString();
            }

            if (sat_timeframes.Count > 1)
            {
                satRatio[1] = (int)(sat_timeframes[1].Item1 * 100);
                satTimes[2] = sat_timeframes[1].Item2.ToString();
                satTimes[3] = sat_timeframes[1].Item3.ToString();
            }

            if (sat_timeframes.Count > 2)
            {
                satRatio[2] = (int)(sat_timeframes[2].Item1 * 100);
                satTimes[4] = sat_timeframes[2].Item2.ToString();
                satTimes[5] = sat_timeframes[2].Item3.ToString();
            }
        }

        private void Display_dept_sunTime()
        {
            if (dept_id == 0) return; // invalid department

            if (sun_timeframes.Count > 0)
            {
                sunRatio[0] = (int)(sun_timeframes[0].Item1 * 100);
                sunTimes[0] = sun_timeframes[0].Item2.ToString();
                sunTimes[1] = sun_timeframes[0].Item3.ToString();
            }

            if (sun_timeframes.Count > 1)
            {
                sunRatio[1] = (int)(sun_timeframes[1].Item1 * 100);
                sunTimes[2] = sun_timeframes[1].Item2.ToString();
                sunTimes[3] = sun_timeframes[1].Item3.ToString();
            }

            if (sun_timeframes.Count > 2)
            {
                sunRatio[2] = (int)(sun_timeframes[2].Item1 * 100);
                sunTimes[4] = sun_timeframes[2].Item2.ToString();
                sunTimes[5] = sun_timeframes[2].Item3.ToString();
            }
        }

        private List<string> Set_pattern(string schedule)
        {
            List<string> template = new List<string> { "", "", "", "", "", "", "" };

            if (schedule.Length == 7)
                for (int i = 0; i < 7; i++)
                    if (schedule[i] != 'X')
                        template[i] = "checked";

            return template;
        }

        private string Get_pattern(string day_key)
        {
            string pat = "";

            if (Request.Form.ContainsKey("mon_w" + day_key)) pat += 'M';
            else pat += 'X';

            if (Request.Form.ContainsKey("tue_w" + day_key)) pat += 'T';
            else pat += 'X';

            if (Request.Form.ContainsKey("wed_w" + day_key)) pat += 'W';
            else pat += 'X';

            if (Request.Form.ContainsKey("thu_w" + day_key)) pat += 'U';
            else pat += 'X';

            if (Request.Form.ContainsKey("fri_w" + day_key)) pat += 'F';
            else pat += 'X';

            if (Request.Form.ContainsKey("sat_w" + day_key)) pat += 'A';
            else pat += 'X';

            if (Request.Form.ContainsKey("sun_w" + day_key)) pat += 'S';
            else pat += 'X';

            return pat;
        }

        private void Display_dept_pattern()
        {
            if (dept_id == 0) return; // invalid department

            // read pattern 1
            if (patterns.Count > 0)
            {
                ratio_w1 = (int)(patterns[0].Item1 * 100);
                weekP1 = Set_pattern(patterns[0].Item2);
            }

            if (patterns.Count > 1)
            {
                ratio_w2 = (int)(patterns[1].Item1 * 100);
                weekP2 = Set_pattern(patterns[1].Item2);
            }

            if (patterns.Count > 2)
            {
                ratio_w3 = (int)(patterns[2].Item1 * 100);
                weekP3 = Set_pattern(patterns[2].Item2);
            }

            if (patterns.Count > 3)
            {
                ratio_w4 = (int)(patterns[3].Item1 * 100);
                weekP4 = Set_pattern(patterns[3].Item2);
            }
        }

        private int Insert_pattern_info(string schedule)
        {
            int pattern_id = 0;
            
            try
            {
                // Open the connection
                database.Open();

                // STEP 1 : we look for an existing pattern
                string findQ = "select schedule_id from schedule_type where pattern = @Pattern";
                MySqlCommand finder = new MySqlCommand(findQ, database);
                finder.Parameters.AddWithValue("@Pattern", schedule);

                object result = finder.ExecuteScalar();

                
                if (result != null)
                    pattern_id = Convert.ToInt32(result);

                else
                {
                    // STEP 2: we insert a new pattern in the DB
                    string insertQ = "INSERT INTO schedule_type (pattern) VALUES (@Pattern)";
                    MySqlCommand insert = new MySqlCommand(insertQ, database);
                    insert.Parameters.AddWithValue("@Pattern", schedule);
                    insert.ExecuteNonQuery();

                    // STEP 3: retrieve the ID of the new pattern
                    pattern_id = (int)finder.ExecuteScalar();
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

            return pattern_id;
        }

        private void Reset_dept_pattern()
        {
            try
            {
                // Open the connection
                database.Open();

                string deletequery = "delete from weekly_business_need where dept_id = @DeptID";
                MySqlCommand delete = new MySqlCommand(deletequery, database);
                delete.Parameters.AddWithValue("@DeptID", dept_id);
                delete.ExecuteNonQuery();

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

        private void Link_to_schedule(string pattern, double ratio)
        {
            int schedule_id = Insert_pattern_info(pattern);
            if (schedule_id == 0) return; // assume error somewhere

            try
            {
                // Open the connection
                database.Open();
                
                string insertQ = "INSERT INTO weekly_business_need (dept_id, schedule_id, ratio) " +
                            "VALUES (@DeptID, @Schedule, @Ratio)";
                MySqlCommand insert = new MySqlCommand(insertQ, database);

                insert.Parameters.AddWithValue("@DeptID", dept_id);
                insert.Parameters.AddWithValue("@Schedule", schedule_id);
                insert.Parameters.AddWithValue("@Ratio", ratio);
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

        private void update_weekly_busyneed()
        {
            // STEP 1 extract the check box
            // STEP 2 create patterns
            // STEP 3 add valid schedules to the list

            string schedP1 = Get_pattern("1");
            double schedR1 = double.Parse(Request.Form["ratio1"]) / 100;
            if (schedP1 != "XXXXXXX" && schedR1 > 0)
                update_patterns.Add(new Tuple<double, string>(schedR1, schedP1));

            string schedP2 = Get_pattern("2");
            double schedR2 = double.Parse(Request.Form["ratio2"]) / 100;
            if (schedP2 != "XXXXXXX" && schedR2 > 0)
                update_patterns.Add(new Tuple<double, string>(schedR2, schedP2));

            string schedP3 = Get_pattern("3");
            double schedR3 = double.Parse(Request.Form["ratio3"]) / 100;
            if (schedP3 != "XXXXXXX" && schedR3 > 0)
                update_patterns.Add(new Tuple<double, string>(schedR3, schedP3));

            string schedP4 = Get_pattern("4");
            double schedR4 = double.Parse(Request.Form["ratio4"]) / 100;
            if (schedP4 != "XXXXXXX" && schedR4 > 0)
                update_patterns.Add(new Tuple<double, string>(schedR4, schedP4));

            // STEP 4: verify that sum of valid ratios are good
            double valid = 0;
            foreach (var schedule in update_patterns)
            {
                valid += schedule.Item1;
            }

            if (valid == 1)
            {
                // STEP 5: update the schedules record
                Reset_dept_pattern();

                foreach (var schedule in update_patterns)
                {
                    Link_to_schedule(schedule.Item2, schedule.Item1);
                }

                message = "Department Needs Updated";
            }
            else message = "You did not enter the schedules correctly!";
        }

        private string Get_timedata(string timeframe)
        {
            string timeinput = Request.Form[timeframe];
            if (timeinput == "") return timeinput;

            TimeSpan timedata = TimeSpan.Parse(timeinput);
            return timedata.ToString();
        }

        private void Reset_dept_timeframe(string day)
        {
            try
            {
                // Open the connection
                database.Open();

                string deletequery = "delete from daily_business_need where dept_id = @DeptID and weekday = @Day";
                MySqlCommand delete = new MySqlCommand(deletequery, database);
                delete.Parameters.AddWithValue("@DeptID", dept_id);
                delete.Parameters.AddWithValue("@Day", day);
                delete.ExecuteNonQuery();

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

        private int Insert_time_info(string stime, string etime)
        {
            int timeframe_id = 0;

            try
            {
                // Open the connection
                database.Open();

                // STEP 1 : we look for an existing pattern
                string findQ = "select timeframe_id from timeframe where " + 
                            "start_time = @Start and end_time = @End";
                MySqlCommand finder = new MySqlCommand(findQ, database);
                finder.Parameters.AddWithValue("@Start", stime);
                finder.Parameters.AddWithValue("@End", etime);

                object result = finder.ExecuteScalar();


                if (result != null)
                    timeframe_id = Convert.ToInt32(result);

                else
                {
                    // STEP 2: we insert a new pattern in the DB
                    string insertQ = "INSERT INTO timeframe (start_time, end_time) VALUES (@Start, @End)";
                    MySqlCommand insert = new MySqlCommand(insertQ, database);
                    insert.Parameters.AddWithValue("@Start", stime);
                    insert.Parameters.AddWithValue("@End", etime);
                    insert.ExecuteNonQuery();

                    // STEP 3: retrieve the ID of the new pattern
                    timeframe_id = (int)finder.ExecuteScalar();
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

            return timeframe_id;
        }

        private void Link_time_to_schedule(string day, double ratio, string stime, string etime)
        {
            int timeframe_id = Insert_time_info(stime, etime);
            if (timeframe_id == 0) return; // assume error somewhere

            try
            {
                // Open the connection
                database.Open();

                string insertQ = "INSERT INTO daily_business_need (dept_id, weekday, timeframe_id, ratio) " +
                            "VALUES (@DeptID, @Day, @TimeID, @Ratio)";
                MySqlCommand insert = new MySqlCommand(insertQ, database);

                insert.Parameters.AddWithValue("@DeptID", dept_id);
                insert.Parameters.AddWithValue("@Day", day);
                insert.Parameters.AddWithValue("@TimeID", timeframe_id);
                insert.Parameters.AddWithValue("@Ratio", ratio);
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
    
        private void update_daily_busyneed(string day)
        {
            // STEP 1 extract the updated times and ratios
            // STEP 2 convert times strings to timespans
            // STEP 3 add valid patterns to the list

            string stime1 = Get_timedata(day + "_t1_st");
            string etime1 = Get_timedata(day + "_t1_et");
            double ratio1 = double.Parse(Request.Form[day + "_ratio1"]) / 100;
            if (stime1 != "" && etime1 != "" && ratio1 > 0)
                update_timeframes.Add(new Tuple<double, string, string>(ratio1, stime1, etime1));

            string stime2 = Get_timedata(day + "_t2_st");
            string etime2 = Get_timedata(day + "_t2_et");
            double ratio2 = double.Parse(Request.Form[day + "_ratio2"]) / 100;
            if (stime2 != "" && etime2 != "" && ratio2 > 0)
                update_timeframes.Add(new Tuple<double, string, string>(ratio2, stime2, etime2));

            string stime3 = Get_timedata(day + "_t3_st");
            string etime3 = Get_timedata(day + "_t3_et");
            double ratio3 = double.Parse(Request.Form[day + "_ratio3"]) / 100;
            if (stime3 != "" && etime3 != "" && ratio3 > 0)
                update_timeframes.Add(new Tuple<double, string, string>(ratio3, stime3, etime3));

            // STEP 4: verify that sum of valid ratios are good
            double valid = 0;
            foreach (var schedule in update_timeframes)
            {
                valid += schedule.Item1;
            }

            if (valid == 1)
            {
                // STEP 5: update the schedules record
                Reset_dept_timeframe(day);

                foreach (var schedule in update_timeframes)
                {
                    Link_time_to_schedule(day, schedule.Item1, schedule.Item2, schedule.Item3);
                }

                time_message = "## " + day + " ## time frames Updated";
            }
            else time_message = "## " + day + " ## time data have not been entered corrrectly";
        }

        public IActionResult OnGet()
        {
            try // verify user
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
            }

            UpdateNotify();

            // try sent user back to index if he tries coming back here without login
            if (user_id == 0)
                return RedirectToPage("Index");

            HttpContext.Session.SetInt32("dept_id", dept_id);

            return Page();
        }

        public IActionResult OnPost()
        {
            try // verify user
            {
                user_id = (int)HttpContext.Session.GetInt32("user_id");
            }
            catch (Exception ex)
            {
                user_id = 0;
            }

            UpdateNotify();

            if (Request.Form.ContainsKey("dept_update"))
            {
                dept_id = int.Parse(Request.Form["dept_id"]);

                HttpContext.Session.SetInt32("dept_id", dept_id);
            }

            try
            {
                // retrieve currrent department ID
                dept_id = (int)HttpContext.Session.GetInt32("dept_id");
            }
            catch (Exception ex)
            {
                dept_id = 0;
            }

            if (Request.Form.ContainsKey("update_busy_need"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_weekly_busyneed();
                addtime = "none";
                addtime_but = "block";
            }

            if (Request.Form.ContainsKey("upd_mon"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_daily_busyneed("mon");
                addtime = "block";
                addtime_but = "none";
            }

            if (Request.Form.ContainsKey("upd_tue"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_daily_busyneed("tue");
                addtime = "block";
                addtime_but = "none";
            }

            if (Request.Form.ContainsKey("upd_wed"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_daily_busyneed("wed");
                addtime = "block";
                addtime_but = "none";
            }

            if (Request.Form.ContainsKey("upd_thu"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_daily_busyneed("thu");
                addtime = "block";
                addtime_but = "none";
            }

            if (Request.Form.ContainsKey("upd_fri"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_daily_busyneed("fri");
                addtime = "block";
                addtime_but = "none";
            }

            if (Request.Form.ContainsKey("upd_sat"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_daily_busyneed("sat");
                addtime = "block";
                addtime_but = "none";
            }

            if (Request.Form.ContainsKey("upd_sun"))
            {
                if (dept_id == 0) message = "Select a Valid department";
                else update_daily_busyneed("sun");
                addtime = "block";
                addtime_but = "none";
            }

            Download_dept_pattern();
            Display_dept_pattern();

            Download_dept_times();
            Display_dept_monTime();
            Display_dept_tueTime();
            Display_dept_wedTime();
            Display_dept_thuTime();
            Display_dept_friTime();
            Display_dept_satTime();
            Display_dept_sunTime();

            if (dept_id > 0)
                dept_name = departments[dept_id];

            
            return Page();
        }
    }
}
