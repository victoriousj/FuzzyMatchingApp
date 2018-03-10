using FuzzyMatchingApp.Models;
using FuzzyMatchingApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyMatchingApp.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			List<Customer> customers = new List<Customer>();
			using (var context = new FuzzyMatchingContext())
			{
				customers = context.Customers.ToList();
			}

			HomePageViewModel model = new HomePageViewModel();
			if (customers != null)
			{
				model.Customers = customers;
			}

			return View(model);
		}
	}
}