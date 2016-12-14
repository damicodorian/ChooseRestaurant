using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChooseRestaurant.Models
{
    public interface IDal : IDisposable
    {
        void CreateRestaurant(string name, string phone);
        void ModifyRestaurant(int id, string name, string phone);
        List<Restaurant> GetAllRestaurants();
        bool RestaurantExists(string name);
        int AddUser(string name, string password);
        UserClass Authentification(string name, string password);
        UserClass GetUser(int id);
        UserClass GetUser(string idStr);
        int CreateSurvey();
        void AddVote(int idSurvey, int idRestaurant, int idUser);
        List<Results> GetResults(int idSurvey);
        bool HasAlreadyVoted(int idSurvey, string idStr);
    }
}
