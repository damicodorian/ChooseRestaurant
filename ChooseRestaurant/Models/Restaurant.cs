using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter the name")]
        public string Name { get; set; }
        [Display(Name = "Phone number")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Incorrect number")]
        public string Phone { get; set; }
    }
}