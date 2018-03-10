using FuzzyMatchingApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FuzzyMatchingApp
{
	public class Context: DbContext
	{
		public DbSet<Customer> Customers { get; set; }
	}
}