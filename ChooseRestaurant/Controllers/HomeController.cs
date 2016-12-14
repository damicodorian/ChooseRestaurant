using ChooseRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChooseRestaurant.Controllers
{
    public class HomeController : Controller
    {
        private IDal dal;

        public HomeController()
            : this(new Dal())
        {
        }

        public HomeController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            int idSurvey = dal.CreateSurvey();
            return RedirectToAction("Index", "Vote", new { id = idSurvey });
        }
    }
}