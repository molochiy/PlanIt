using System.ComponentModel.DataAnnotations;

namespace PlanIt.Web.Models
{
    public class SignUpDataViewModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your passqord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your passqord one more time")]
        public string ConfirmPassword { get; set; }
    }
}