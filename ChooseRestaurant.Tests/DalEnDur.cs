using ChooseRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChooseRestaurant.Tests
{
    class DalEnDur : IDal
    {
        private List<Restaurant> listOfRestaurants;
        private List<UserClass> listOfUsers;
        private List<Survey> listOfSurveys;

        public DalEnDur()
        {
            listOfRestaurants = new List<Restaurant>
        {
            new Restaurant { Id = 1, Name = "La bonne fourchette", Phone = "0102030405"},
            new Restaurant { Id = 2, Name = "Resto pinière", Phone = "0102030405"},
            new Restaurant { Id = 3, Name = "Resto toro", Phone = "0102030405"},
        };
            listOfUsers = new List<UserClass>();
            listOfSurveys = new List<Survey>();
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return listOfRestaurants;
        }

        public void CreateRestaurant(string name, string phone)
        {
            int id = listOfRestaurants.Count == 0 ? 1 : listOfRestaurants.Max(r => r.Id) + 1;
            listOfRestaurants.Add(new Restaurant { Id = id, Name = name, Phone = phone });
        }

        public void ModifyRestaurant(int id, string name, string phone)
        {
            Restaurant restaurant = listOfRestaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                restaurant.Name = name;
                restaurant.Phone = phone;
            }
        }

        public bool RestaurantExists(string name)
        {
            return listOfRestaurants.Any(r => string.Compare(r.Name, name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public int AddUser(string name, string password)
        {
            int id = listOfUsers.Count == 0 ? 1 : listOfUsers.Max(u => u.Id) + 1;
            listOfUsers.Add(new UserClass { Id = id, Name = name, Password = password });
            return id;
        }

        public UserClass Authentification(string name, string password)
        {
            return listOfUsers.FirstOrDefault(u => u.Name == name && u.Password == password);
        }

        public UserClass GetUser(int id)
        {
            return listOfUsers.FirstOrDefault(u => u.Id == id);
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
            int id = listOfSurveys.Count == 0 ? 1 : listOfSurveys.Max(s => s.Id) + 1;
            listOfSurveys.Add(new Survey { Id = id, Date = DateTime.Now, Votes = new List<Vote>() });
            return id;
        }

        public void AddVote(int idSurvey, int idRestaurant, int idUser)
        {
            Vote vote = new Vote
            {
                Restaurant = listOfRestaurants.First(r => r.Id == idRestaurant),
                User = listOfUsers.First(u => u.Id == idUser)
            };
            Survey survey = listOfSurveys.First(s => s.Id == idSurvey);
            survey.Votes.Add(vote);
        }

        public bool HasAlreadyVoted(int idSurvey, string idStr)
        {
            UserClass user = GetUser(idStr);
            if (user == null)
                return false;
            Survey survey = listOfSurveys.First(s => s.Id == idSurvey);
            return survey.Votes.Any(v => v.User.Id == user.Id);
        }

        public List<Results> GetResults(int idSondage)
        {
            List<Restaurant> restaurants = GetAllRestaurants();
            List<Results> resultats = new List<Results>();
            Survey sondage = listOfSurveys.First(s => s.Id == idSondage);
            foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.Restaurant.Id))
            {
                int idRestaurant = grouping.Key;
                Restaurant restaurant = restaurants.First(r => r.Id == idRestaurant);
                int numberOfVotes = grouping.Count();
                resultats.Add(new Results { Name = restaurant.Name, Phone = restaurant.Phone, NumberOfVotes = numberOfVotes });
            }
            return resultats;
        }

        public void Dispose()
        {
            listOfRestaurants = new List<Restaurant>();
            listOfUsers = new List<UserClass>();
            listOfSurveys = new List<Survey>();
        }
    }
}
