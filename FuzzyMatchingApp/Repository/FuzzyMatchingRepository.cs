using FuzzyMatchingApp.Models;
using System;
using System.Collections.Generic;
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
				customers = context.Customers
					.Where(c => c.FirstName.Contains(term) 
					|| c.LastName.Contains(term))
					.ToList();
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