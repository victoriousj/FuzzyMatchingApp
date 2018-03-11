namespace FuzzyMatchingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "MiddleName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "MiddleName", c => c.String());
        }
    }
}
