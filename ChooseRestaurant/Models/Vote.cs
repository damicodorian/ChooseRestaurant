using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual UserClass User { get; set; }
    }
}