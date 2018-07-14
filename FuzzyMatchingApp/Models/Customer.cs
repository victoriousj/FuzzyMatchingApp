using System;

namespace FuzzyMatchingApp.Models
{
	public class Customer
	{
		public Customer() { }

		public Customer(string firstName, string lastName)
		{
			FirstName = firstName ?? throw new ArgumentNullException();
			LastName = lastName ?? throw new ArgumentNullException();
		}

		public int ID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }

		public string PhoneNumber { get; set; }
	}
}