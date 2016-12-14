using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.ViewModels
{
    public class RestaurantCheckBoxViewModel
    {
        public int Id { get; set; }
        public string NameAndPhone { get; set; }
        public bool IsSelected { get; set; }
    }
}