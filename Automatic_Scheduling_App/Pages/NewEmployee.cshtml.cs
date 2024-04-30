using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System;
using System.Text;
using System.Windows;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using System.Reflection.PortableExecutable;


namespace Automatic_Scheduling_App.Pages
{
    public class NewEmployee: PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string msg {get;set;}
        public NewEmployee(ILogger<IndexModel> logger)
        {
            _logger = logger;
            msg = "";
        }
        public void OnGet()
        {

        }
        private string  Add_Employee(object sender, EventArgs e)
        {
            string user_id = Request.Form["user_id"];
            string first_name = Request.Form["first_name"];
            string last_name = Request.Form["last_name"];
            string email = Request.Form["email"];
            string position = Request.Form["position"];
            string street = Request.Form["street"];
            string city = Request.Form["city"];
            string unit = Request.Form["unit"];
            string state = Request.Form["state"];
            string zipcode= Request.Form["zipcode"];
            string dept_id = Request.Form["dept_id"];

            MySqlConnection con = new MySqlConnection("server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;user=admin;password=adminroot;database=schooldb;port=3306");
            MySqlCommand cmd = new MySqlCommand("INSERT INTO staff(user_id,first_name,last_name,email,position,city,unit,state,zipcode,dept_id),VALUES(@user_id,@first_name,@last_name,@email,@position,@city,@unit,@state,@zipcode,@dept_id)",con);
            
            cmd.Parameters.AddWithValue("@user_id",user_id);
            cmd.Parameters.AddWithValue("@first_name",first_name);
            cmd.Parameters.AddWithValue("@last_name",last_name);
            cmd.Parameters.AddWithValue("@email",email);
            cmd.Parameters.AddWithValue("@position",position);
            cmd.Parameters.AddWithValue("@street",street);
            cmd.Parameters.AddWithValue("@city",city);
            cmd.Parameters.AddWithValue("@unit",unit);
            cmd.Parameters.AddWithValue("@state",state);
            cmd.Parameters.AddWithValue("@zipcode",zipcode);
            cmd.Parameters.AddWithValue("@dept_id",dept_id);
            con.Open();
            // make sure we create everything needed before puting something in the database
            // use print to console instead to see that it works
            // cmd.ExecuteNonQuery(); 
            con.Close();
            msg = "Employee Sucessfully Added";
            return msg;
            
            

        }
    }
}