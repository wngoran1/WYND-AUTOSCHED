using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System;
using System.Text;
using System.Windows;
using System.Data;


namespace Automatic_Scheduling_App.Pages
{
    public class createUserModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public createUserModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        private void CreateUser(object sender, EventArgs e)
        {
            string user_id = Request.Form["user_id"];
           
            string email = Request.Form["email"];

             MySqlConnection con = new MySqlConnection("server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;user=admin;password=adminroot;database=schooldb;port=3306");

            if(user_id !=string.Empty || email !=string.Empty) //to make sure that they put in something for all three
            {
               
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM staff WHERE user_id= '"+ user_id+"'",con);
                MySqlDataReader dr = cmd.ExecuteReader();

                if(dr.Read())//if there is an existing data with the same username
                {
                    dr.Close();
                    Console.WriteLine("Account already exists.\n");
                  
                }
                else
                {
                    MySqlCommand CMD= new MySqlCommand("INSERT INTO staff(user_id,email) VALUES(@user_id,@email)",con);
                    CMD.Parameters.AddWithValue("@user_id",user_id);

                    
                    CMD.Parameters.AddWithValue("@email",email);
                    con.Open();
                    CMD.ExecuteNonQuery();
                    con.Close();
                }
            }
            else
            {

            }
    }
    }
}