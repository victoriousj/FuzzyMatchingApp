using FuzzyMatchingApp.Helpers;
using FuzzyMatchingApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace FuzzyMatchingApp.Repository
{
	public class FuzzyMatchingRepository
	{
		public List<Customer> FetchAllCustomers()
		{
			var customers = new List<Customer>();
			using (var context = new FuzzyMatchingContext())
			{
				customers = context.Customers.ToList();
			}
			return customers;
		}

		public Customer FetchCustomer(int id)
		{
			if (id == 0) return null;

			var Customer = new Customer();
			using (var context = new FuzzyMatchingContext())
			{
				Customer = context.Customers
				.Where(c => c.ID == id)
				.FirstOrDefault();
			}
			return Customer;
		}

		public List<Customer> SearchCustomersByName(string term)
		{
			if (term == null) return null;

			List<Customer> customers = new List<Customer>();
			using (var context = new FuzzyMatchingContext())
			{
				customers.AddRange(
					context.Customers
					.Where(c => 
						   SqlFunctions.PatIndex("%"+term+"%", c.FirstName) > 0
						|| SqlFunctions.PatIndex("%"+term+"%", c.LastName) > 0
						|| SqlFunctions.SoundCode(c.FirstName).Contains(SqlFunctions.SoundCode(term))
						|| SqlFunctions.SoundCode(c.LastName).Contains(SqlFunctions.SoundCode(term)))
					.Take(40).ToList());


				customers.AddRange(context.Customers.OrderByDescending(c => SqlFunctions.Difference(c.LastName, term)).Take(40).ToList());
			}
			return customers;
		}

		public Customer SearchCustomerByName(string term)
		{
			if (term == null) return null;

			var customer = new Customer();
			using (var context = new FuzzyMatchingContext())
			{
				customer = context.Customers
					.Where(c => c.FirstName.ToLower().Contains(term.ToLower()))
					.FirstOrDefault();
			}
			return customer;
		}
	}
}