using FuzzyMatchingApp.Models;
using System.Data.Entity;

namespace FuzzyMatchingApp
{
	public class FuzzyMatchingContext: DbContext
	{
		public FuzzyMatchingContext() : base(nameof(FuzzyMatchingContext)) { }

		public DbSet<Customer> Customers { get; set; }
	}
}