using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Automatic_Scheduling_App.Controllers
{
    public class DepartmentController : Controller
    {
        [HttpPost]
        public IActionResult AddDepartment(string deptName)
        {
            Console.WriteLine(deptName);
            string connectionString = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query1 = "INSERT INTO department (dept_name) VALUES ('" + deptName + "');";
                MySqlCommand command1 = new MySqlCommand(query1, connection);
                int rowsAffected = command1.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Data inserted successfully.");
                }
                else
                {
                    Console.WriteLine("No rows inserted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Close connection
                connection.Close();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}