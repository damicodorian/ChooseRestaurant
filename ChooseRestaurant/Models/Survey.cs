using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual List<Vote> Votes { get; set; }
    }
}