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
using System.Security.Cryptography.X509Certificates;


namespace Automatic_Scheduling_App.Pages
{
    public class createUserModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string db_config = "server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;port=3306;database=schedule_app;uid=admin;password=adminroot;";
        private MySqlConnection database { get; set; }
        public string manager { get; set; }
        public string signin { get; set; }
         public string msg {get;set;}
        public createUserModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            signin = "LogOut";
            manager = "block";
            msg = "";
           
        }

        public void OnGet()
        {
            
        }
        private string CreateUser(object sender, EventArgs e)
        {
            string user_id = Request.Form["user_id"];
            string password = Request.Form["password"];
            string email = Request.Form["email"];

             MySqlConnection con = new MySqlConnection("server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;user=admin;password=adminroot;database=schooldb;port=3306");

            if(user_id !=string.Empty || email !=string.Empty) //to make sure that they put in something for all three
            {
               
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM staff WHERE user_id= '"+ user_id+"'",con);
                MySqlDataReader dr = cmd.ExecuteReader();

                if(dr.Read())//if there is an existing data with the same username
                {
                    dr.Close();
                   string msg = "Account Already Exists";
                   return msg;
                  
                }
                else
                {
                    MySqlCommand CMD= new MySqlCommand("INSERT INTO staff(user_id,email) VALUES(@user_id,@email)",con);
                    CMD.Parameters.AddWithValue("@user_id",user_id);
                    CMD.Parameters.AddWithValue("!password",password);
                    CMD.Parameters.AddWithValue("@email",email);
                    con.Open();
                    CMD.ExecuteNonQuery();
                    con.Close();
                     msg = "Account Successfully created";
                    return msg;
                }
            }
            else
            {
                msg = "Please Fill in data";
                return msg;
            }
    }
    }
}
