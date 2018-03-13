namespace FuzzyMatchingApp.Migrations
{
	using FuzzyMatchingApp.Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.IO;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<FuzzyMatchingContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "FuzzyMatchingApp.FuzzyMatchingContext";
		}

		static string path = Path.GetFullPath(@"..\..\");
		static string LASTNAMES = path + @"Resources\lastNames.txt";
		static string FIRSTNAMES = path + @"Resources\maleAndFemaleNames.txt";
		static string ADDRESSES = path + @"Resources\addresses.txt";
		static string PHONE_NUMBERS = path += @"Resources\randomTelephoneNumbers.txt";
		static Random random = new Random();

		// Generate 10,000 randomly named Customer entities.
		protected override void Seed(FuzzyMatchingContext context)
		{
			string firstNamesString = File.ReadAllText(FIRSTNAMES);
			List<string> firstNameList = firstNamesString.Split(' ').ToList();
			int firstNameCount = firstNameList.Count();

			string lastNamesString = File.ReadAllText(LASTNAMES);
			List<string> lastNameList = lastNamesString.Split(' ').ToList();
			int lastNameCount = lastNameList.Count();

			string addressesString = File.ReadAllText(ADDRESSES);
			List<string> addressList = addressesString.Split('|').ToList();
			int addressesCount = addressList.Count();

			string phoneNumberString = File.ReadAllText(PHONE_NUMBERS);
			List<string> phoneNumberList = phoneNumberString.Split(' ').ToList();
			int phoneNumberCount = phoneNumberList.Count();

			List<Customer> customers = new List<Customer>();
			for (int i = 0; i <= 10000; i++)
			{
				string randomFirstName = firstNameList[random.Next(firstNameCount)];
				string randomLastName = lastNameList[random.Next(lastNameCount)];
				string randomAddress = addressList[random.Next(addressesCount)];
				string randomPhoneNumber = phoneNumberList[random.Next(phoneNumberCount)];

				Customer customer = new Customer
				{
					FirstName = randomFirstName,
					LastName = randomLastName,
					Address = randomAddress,
					PhoneNumber = randomPhoneNumber,
				};
				customers.Add(customer);
			}
			context.Customers.AddOrUpdate(customers.ToArray());
		}
	}
}
