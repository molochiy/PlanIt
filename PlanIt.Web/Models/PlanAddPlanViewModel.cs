﻿using System.ComponentModel.DataAnnotations;
using System;


namespace PlanIt.Web.Models
{
    public class PlanAddPlanViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter Title")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public Nullable<int> PlanId { get; set; }
    }
}