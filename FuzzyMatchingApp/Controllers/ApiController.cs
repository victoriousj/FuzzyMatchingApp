using FuzzyMatchingApp.Models;
using FuzzyMatchingApp.Service;
using System.Linq;
using System.Web.Mvc;

namespace FuzzyMatchingApp.Controllers
{
	public class ApiController : Controller
    {
		private FuzzyMatchingService _service { get; set; }

		public ApiController()
		{
			_service = new FuzzyMatchingService();
		}

		public JsonResult CustomerSearch(string term)
		{
			var customer = new Customer();
			customer = _service.FetchCustomerByName(term);
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
			var customers = _service.FetchCustomersByName(term);

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
			return Json(new { });
		}

		public JsonResult GetCustomerAddress(string id)
		{
			var customer = _service.FetchCustomerById(int.Parse(id));

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