using FuzzyMatchingApp.Models;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace FuzzyMatchingApp.Repository
{
	public class FuzzyMatchingRepository
	{
		private FuzzyMatchingContext _context;

		public FuzzyMatchingRepository()
		{
			_context = new FuzzyMatchingContext();
		}

		public List<Customer> FetchAllCustomers()
		{
			var customers = new List<Customer>();
			customers = _context.Customers.ToList();
			return customers;
		}

		public Customer FetchCustomer(int id)
		{
			if (id == 0) return null;

			return _context.Customers
				.Where(c => c.ID == id)
				.FirstOrDefault();
		}

		public List<Customer> SearchCustomersByName(string term)
		{
			if (term == null) return null;

			var customers = new List<Customer>();
			customers.AddRange(
				_context.Customers
				.Where(c => 
						SqlFunctions.PatIndex("%"+term+"%", c.FirstName) > 0
					|| SqlFunctions.PatIndex("%"+term+"%", c.LastName) > 0
					|| SqlFunctions.SoundCode(c.FirstName).Contains(SqlFunctions.SoundCode(term))
					|| SqlFunctions.SoundCode(c.LastName).Contains(SqlFunctions.SoundCode(term)))
				.Take(40).ToList());

			customers.AddRange(_context.Customers.OrderByDescending(c => SqlFunctions.Difference(c.LastName, term)).Take(40).ToList());

			return customers;
		}

		public Customer SearchCustomerByName(string term)
		{
			if (term == null) return null;

			return _context.Customers
				.Where(c => c.FirstName.ToLower().Contains(term.ToLower()))
				.FirstOrDefault();
		}
	}
}