using FuzzyMatchingApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FuzzyMatchingApp
{
	public class FuzzyMatchingSeed: DropCreateDatabaseIfModelChanges<FuzzyMatchingContext>
	{
		protected override void Seed(FuzzyMatchingContext context)
		{
			List<Customer> customer = new List<Customer>()
			{
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
				new Customer("Victor", "Donald", "Johnson"),
			};
		}
	}
}