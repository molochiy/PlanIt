using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanIt.Web.Models
{
    public class SignUpDataViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}