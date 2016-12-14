using ChooseRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChooseRestaurant.Controllers
{
    public class RestaurantController : Controller
    {
        private IDal dal;

        public RestaurantController()
            : this(new Dal())
        {
        }

        public RestaurantController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            List<Restaurant> listOfRestaurants = dal.GetAllRestaurants();
            return View(listOfRestaurants);
        }

        public ActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRestaurant(Restaurant restaurant)
        {
            if (dal.RestaurantExists(restaurant.Name))
            {
                ModelState.AddModelError("Name", "This name already exists");
                return View(restaurant);
            }
            if (!ModelState.IsValid)
                return View(restaurant);
            dal.CreateRestaurant(restaurant.Name, restaurant.Phone);
            return RedirectToAction("Index");
        }

        public ActionResult ModifyRestaurant(int? id)
        {
            if (id.HasValue)
            {
                Restaurant restaurant = dal.GetAllRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ModifyRestaurant(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
                return View(restaurant);
            dal.ModifyRestaurant(restaurant.Id, restaurant.Name, restaurant.Phone);
            return RedirectToAction("Index");
        }
    }
}