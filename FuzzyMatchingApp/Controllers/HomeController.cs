using FuzzyMatchingApp.Models.ViewModels;
using System.Web.Mvc;

namespace FuzzyMatchingApp.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index() =>
			View(new HomePageViewModel());
	}
}