using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyMatchingApp.Models
{
	public class Customer
	{
		public Customer() { }

		public Customer(string firstName, string middleName, string lastName)
		{
			FirstName = firstName;
			MiddleName = middleName;
			LastName = lastName;
		}

		public int ID { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }
	}
}