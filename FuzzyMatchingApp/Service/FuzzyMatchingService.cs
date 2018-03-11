using FuzzyMatchingApp.Models;
using FuzzyMatchingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyMatchingApp.Service
{
	public class FuzzyMatchingService
	{
		private FuzzyMatchingContext context { get; set; }
		private FuzzyMatchingRepository repository { get; set; }

		public FuzzyMatchingService(FuzzyMatchingContext context)
		{
			this.context = context;
			repository = new FuzzyMatchingRepository();
		}

		public List<Customer> FetchAllCustomers()
		{
			var customers = new List<Customer>();
			return customers = repository.FetchAllCustomers();
		}

		public Customer FetchCustomerById(int id)
		{
			var customer = new Customer();
			return customer = repository.FetchCustomer(id);
		}

		public Customer FetchCustomerByName(string term)
		{
			var customer = new Customer();
			return customer = repository.SearchCustomerByName(term);
		}
		
		public List<Customer> FetchCustomersByName(string term)
		{
			var customers = new List<Customer>();
			return customers = repository.SearchCustomersByName(term);
		}
	}
}