using FuzzyMatchingApp.Models;
using FuzzyMatchingApp.Repository;
using FuzzyMatchingApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyMatchingApp.Controllers
{
    public class ApiController : Controller
    {
		private FuzzyMatchingContext context { get; set; }
		private FuzzyMatchingService service { get; set; }

		public ApiController()
		{
			context = new FuzzyMatchingContext();
			service = new FuzzyMatchingService(context);
		}

		public JsonResult CustomerSearch(string term)
		{
			var customer = new Customer();
			customer = service.FetchCustomerByName(term);
			if (customer != null)
			{
				return Json(new {
					firstName = customer.FirstName,
					lastName = customer.LastName,
					id = customer.ID },
					JsonRequestBehavior.AllowGet);
			}
			return Json(new { });
		}

		public JsonResult CustomersSearch(string term)
		{
			term = term.ToLower().Replace(',', ' ');
			var terms = term.Split(' ');

			var customers = new List<Customer>();
			if (terms.Length > 1)
			{
				foreach (var singleTerm in terms)
				{
					customers.AddRange(service.FetchCustomersByName(singleTerm));
				}
			}
			else
			{
				customers = service.FetchCustomersByName(term);
			}


			if (customers.Any())
			{
				return Json(customers, JsonRequestBehavior.AllowGet);
			}
			return Json(new object { });
		}
    }
}