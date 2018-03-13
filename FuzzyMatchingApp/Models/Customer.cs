using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyMatchingApp.Models
{
	public class Customer
	{
		public Customer() { }

		public Customer(string firstName, string lastName)
		{
			FirstName = firstName;
			LastName = lastName;
		}

		public int ID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }
	}
}