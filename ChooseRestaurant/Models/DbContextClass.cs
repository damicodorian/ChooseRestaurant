using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class DbContextClass : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<UserClass> Users { get; set; }
    }
}