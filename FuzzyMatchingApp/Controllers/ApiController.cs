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
					address = customer.Address,
					id = customer.ID },
					JsonRequestBehavior.AllowGet);
			}
			return Json(new { });
		}

		public JsonResult CustomersSearch(string term)
		{
			var customers = service.FetchCustomersByName(term);

			if (customers.Any())
			{
				return Json(customers.Select(c => new
				{
					firstName = c.FirstName,
					lastName = c.LastName,
					address = c.Address,
					id = c.ID,
				}), JsonRequestBehavior.AllowGet);
			}
			return Json(new object { });
		}

		public JsonResult GetCustomerAddress(string id)
		{
			var customer = service.FetchCustomerById(int.Parse(id));

			return Json(new
			{
				firstName = customer.FirstName,
				lastName = customer.LastName,
				address = customer.Address,
				phoneNumber = customer.PhoneNumber,
			}, JsonRequestBehavior.AllowGet);
		}

	}
}