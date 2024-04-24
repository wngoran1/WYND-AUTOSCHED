using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System;
using System.Text;
using System.Windows;
using System.Data;


namespace Automatic_Scheduling_App.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public LoginModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
         private void Login_Button(object sender, EventArgs e)
        {
            string user_id = Request.Form["user_id"];
            
            MySqlConnection con = new MySqlConnection("server=wnprojectdb.chyi8sy82mh3.us-east-2.rds.amazonaws.com;user=admin;password=adminroot;database=schooldb;port=3306");
            MySqlCommand cmd = new MySqlCommand("SELECT FROM staff WHERE user_id = @user_id",con);
            cmd.Parameters.AddWithValue("@user_id",user_id);
            
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds,"staff");
            if(ds.Tables["staff"].Rows.Count==0)
            {
                Console.WriteLine("Invalid Login");
            }
            else
            {
            
            }
        }
    

    }
}