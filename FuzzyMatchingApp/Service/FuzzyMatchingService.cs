using FuzzyMatchingApp.Helpers;
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
			term = term.ToLower().Replace(',', ' ');
			var terms = term.Split(' ');

			var customers = new List<Customer>();
			foreach (var singleTerm in terms)
			{
				customers.AddRange(repository.SearchCustomersByName(singleTerm));
			}
			customers = OrderCustomerResults(term, customers).ToList();

			return customers;
		}

		private ICollection<Customer> OrderCustomerResults(string term, ICollection<Customer> customers)
		{
			// Sterilize the search term
			term = term
				.Replace(",", String.Empty)
				.Trim()
				.ToLower();

			// To search what we presume would be first/last name combinations
			string[] terms = term.Split(' ')
				.Where(x => x.Length > 1)
				.ToArray();

			// For multiple phrases entered at once 'Von Ha Fredrick'
			// 'Von Ha' would be searched together and 'Ha Fredrick' as well
			var linkedTerms = new List<string>();
			if (terms.Length > 2)
			{
				for (int i = 0; i < terms.Length; i++)
				{
					if (terms.Length > i + 1)
					{
						linkedTerms.Add($"{terms[i]} {terms[i + 1]}");
					}
				}
			}

			customers = customers
				.GroupBy(c => c.ID)
				.Select(cd => cd.FirstOrDefault())
				.OrderByDescending(c => term.Equals($"{c.FirstName?.ToLower()} {c.LastName?.ToLower()}") || term.Equals($"{c.LastName?.ToLower()} {c.FirstName?.ToLower()}"))

				.ThenByDescending(c => term.Equals(c.FirstName?.ToLower()))
				.ThenByDescending(c => term.Equals(c.LastName?.ToLower()))

				.ThenByDescending(c => linkedTerms.Any(x => x.Equals(c.FirstName?.ToLower())))
				.ThenByDescending(c => linkedTerms.Any(x => x.Equals(c.LastName?.ToLower())))

				.ThenByDescending(c => linkedTerms.Any(x => c.FirstName != null && c.FirstName.ToLower().Contains(x)))
				.ThenByDescending(c => linkedTerms.Any(x => c.LastName != null && c.LastName.ToLower().Contains(x)))

				.ThenByDescending(c => c.FirstName?.ToLower().Contains(term))
				.ThenByDescending(c => c.LastName?.ToLower().Contains(term))

				.ThenByDescending(c => terms.Any(x => x.Equals(c.FirstName?.ToLower())))
				.ThenByDescending(c => terms.Any(x => x.Equals(c.LastName?.ToLower())))

				.ThenByDescending(c => terms.Any(x => c.FirstName != null && c.FirstName.ToLower().Contains(x)))
				.ThenByDescending(c => terms.Any(x => c.LastName != null && c.LastName.ToLower().Contains(x)))

				.ThenBy(c => terms.Length > 1
				? FuzzyMatchingHelper.LevenshteinsDifference(new string[] { c.FirstName.ToLower(), c.LastName.ToLower() }, terms)
				: FuzzyMatchingHelper.LevenshteinsDifference(FuzzyMatchingHelper.FindBetterMatch(term, c.FirstName.ToLower(), c.LastName.ToLower()), term))

				.ThenBy(c => c.LastName ?? null)
				.ThenBy(c => c.FirstName ?? null)

				.ToList();

			return customers;
		}
	}
}