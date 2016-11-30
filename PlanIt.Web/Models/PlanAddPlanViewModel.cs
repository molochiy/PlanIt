using System.ComponentModel.DataAnnotations;
using System;


namespace PlanIt.Web.Models
{
    public class PlanAddPlanViewModel
    {
        [Required(ErrorMessage = "Please enter Title")]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}