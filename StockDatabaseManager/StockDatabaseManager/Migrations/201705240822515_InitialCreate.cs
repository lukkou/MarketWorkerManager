namespace StockDatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockMasters",
                c => new
                    {
                        StockCode = c.Int(nullable: false, identity: true),
                        StockName = c.String(unicode: false),
                        MarketName = c.String(unicode: false),
                        IndustryCode33 = c.Int(nullable: false),
                        IndustryCode17 = c.Int(nullable: false),
                        ClassCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockMasters");
        }
    }
}
