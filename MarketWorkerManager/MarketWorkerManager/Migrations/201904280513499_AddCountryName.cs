namespace MarketWorkerManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndexCalendars", "CountryName", c => c.String(maxLength: 2048, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IndexCalendars", "CountryName");
        }
    }
}
