﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Automatic_Scheduling_App.Pages
{

    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public string manager { get; set; }
        public string userValid { get; set; }
        public string signin { get; set; }
        public int notify { get; set; }
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            manager = "none";
            userValid = "none";
            signin = "WYND Solutions";
        }

        public void OnGet()
        {
        }
    }

}
