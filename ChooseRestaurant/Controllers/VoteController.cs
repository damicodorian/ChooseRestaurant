using ChooseRestaurant.Models;
using ChooseRestaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChooseRestaurant.Controllers
{
    public class VoteController : Controller
    {
        private IDal dal;

        public VoteController()
            : this(new Dal())
        {
        }

        public VoteController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index(int id)
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListOfRestaurants = dal.GetAllRestaurants().Select(r => new RestaurantCheckBoxViewModel { Id = r.Id, NameAndPhone = string.Format("{0} ({1})", r.Name, r.Phone) }).ToList()
            };
            if (dal.HasAlreadyVoted(id, Request.Browser.Browser))
            {
                return RedirectToAction("ShowResult", new { id = id });
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            UserClass user = dal.GetUser(Request.Browser.Browser);
            if (user == null)
                return new HttpUnauthorizedResult();
            foreach (RestaurantCheckBoxViewModel restaurantCheckBoxViewModel in viewModel.ListOfRestaurants.Where(r => r.IsSelected))
            {
                dal.AddVote(id, restaurantCheckBoxViewModel.Id, user.Id);
            }
            return RedirectToAction("ShowResult", new { id = id });
        }

        public ActionResult ShowResult(int id)
        {
            if (!dal.HasAlreadyVoted(id, Request.Browser.Browser))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Results> results = dal.GetResults(id);
            return View(results.OrderByDescending(r => r.NumberOfVotes).ToList());
        }
    }
}