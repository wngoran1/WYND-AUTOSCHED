using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

namespace Automatic_Scheduling_App.Pages
{
    public class Departments
    {
        private string connectionString = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        public List<string> DeptName { get; set; }

        public Departments()
        {
            DeptName = new List<string>();
            LoadDepartments();
        }
        public Departments(string dept_name)
        {
            DeptName = [dept_name];
        }
        public void AddDepartment(string deptName)
        {
            DeptName.Add(deptName);
        }

        public void RemoveDepartment(string deptName)
        {
            DeptName.Remove(deptName);
        }
        private void LoadDepartments()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT dept_name FROM department";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string deptName = reader.GetString("dept_name");
                                DeptName.Add(deptName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
    public class BusinessNeedsModel : PageModel
    {
        public Departments AllDepts { get; private set; }
        public void OnGet()
        {
            AllDepts = new Departments();
        }
    }
}
