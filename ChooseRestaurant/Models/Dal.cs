using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class Dal : IDal
    {
        private DbContextClass db;

        public Dal()
        {
            db = new DbContextClass();
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return db.Restaurants.ToList();
        }

        public void CreateRestaurant(string name, string phone)
        {
            db.Restaurants.Add(new Restaurant { Name = name, Phone = phone });
            db.SaveChanges();
        }

        public void ModifyRestaurant(int id, string name, string phone)
        {
            Restaurant restaurant = db.Restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                restaurant.Name = name;
                restaurant.Phone = phone;
                db.SaveChanges();
            }
        }

        public bool RestaurantExists(string name)
        {
            return db.Restaurants.Any(r => string.Compare(r.Name, name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public int AddUser(string name, string password)
        {
            string passwordEncoded = EncodeMD5(password);
            UserClass user = new UserClass { Name = name, Password = passwordEncoded };
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public UserClass Authentification(string name, string password)
        {
            string passwordEncoded = EncodeMD5(password);
            return db.Users.FirstOrDefault(u => u.Name == name && u.Password == passwordEncoded);
        }

        public UserClass GetUser(int id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        public UserClass GetUser(string idStr)
        {
            int id;
            if (int.TryParse(idStr, out id))
                return GetUser(id);
            return null;
        }

        public int CreateSurvey()
        {
            Survey survey = new Survey { Date = DateTime.Now };
            db.Surveys.Add(survey);
            db.SaveChanges();
            return survey.Id;
        }

        public void AddVote(int idSurvey, int idRestaurant, int idUser)
        {
            Vote vote = new Vote
            {
                Restaurant = db.Restaurants.First(r => r.Id == idRestaurant),
                User = db.Users.First(u => u.Id == idUser)
            };
            Survey survey = db.Surveys.First(s => s.Id == idSurvey);
            if (survey.Votes == null)
                survey.Votes = new List<Vote>();
            survey.Votes.Add(vote);
            db.SaveChanges();
        }

        public List<Results> GetResults(int idSurvey)
        {
            List<Restaurant> restaurants = GetAllRestaurants();
            List<Results> results = new List<Results>();
            Survey survey = db.Surveys.First(s => s.Id == idSurvey);
            foreach (IGrouping<int, Vote> grouping in survey.Votes.GroupBy(v => v.Restaurant.Id))
            {
                int idRestaurant = grouping.Key;
                Restaurant restaurant = restaurants.First(r => r.Id == idRestaurant);
                int numberOfVotes = grouping.Count();
                results.Add(new Results { Name = restaurant.Name, Phone = restaurant.Phone, NumberOfVotes = numberOfVotes });
            }
            return results;
        }

        public bool HasAlreadyVoted(int idSurvey, string idStr)
        {
            int id;
            if (int.TryParse(idStr, out id))
            {
                Survey survey = db.Surveys.First(s => s.Id == idSurvey);
                if (survey.Votes == null)
                    return false;
                return survey.Votes.Any(v => v.User != null && v.User.Id == id);
            }
            return false;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        private string EncodeMD5(string password)
        {
            string passwordSel = "ChooseRestaurant" + password + "Application";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(passwordSel)));
        }
    }
}