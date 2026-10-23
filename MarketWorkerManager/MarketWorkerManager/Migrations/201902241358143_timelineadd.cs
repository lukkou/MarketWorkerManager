namespace MarketWorkerManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timelineadd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExchangeTimeLines",
                c => new
                    {
                        symbol = c.String(nullable: false, maxLength: 6, storeType: "nvarchar"),
                        tradedatetime = c.DateTime(nullable: false, precision: 0),
                        timespan = c.Int(nullable: false),
                        open = c.Double(nullable: false),
                        high = c.Double(nullable: false),
                        low = c.Double(nullable: false),
                        close = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.symbol, t.tradedatetime, t.timespan });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExchangeTimeLines");
        }
    }
}
