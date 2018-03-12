using FuzzyMatchingApp.Models;
using FuzzyMatchingApp.Models.ViewModels;
using FuzzyMatchingApp.Repository;
using FuzzyMatchingApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyMatchingApp.Controllers
{
	public class HomeController : Controller
	{
		private FuzzyMatchingContext context { get; set; }
		private FuzzyMatchingService service { get; set; }

		public HomeController ()
		{
			context = new FuzzyMatchingContext();
			service = new FuzzyMatchingService(context);
		}

		public ActionResult Index()
		{
			HomePageViewModel model = new HomePageViewModel();
			return View(model);
		}
	}
}