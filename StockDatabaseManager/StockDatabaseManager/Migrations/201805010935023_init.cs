namespace StockDatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CandleSticks",
                c => new
                    {
                        stockcode = c.String(nullable: false, maxLength: 4, storeType: "nvarchar"),
                        businessday = c.DateTime(nullable: false, precision: 0),
                        open = c.Double(nullable: false),
                        high = c.Double(nullable: false),
                        low = c.Double(nullable: false),
                        close = c.Double(nullable: false),
                        volume = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.stockcode, t.businessday });
            
            CreateTable(
                "dbo.ClassMasters",
                c => new
                    {
                        code = c.String(nullable: false, maxLength: 2, storeType: "nvarchar"),
                        name = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.code);
            
            CreateTable(
                "dbo.IndexCalendars",
                c => new
                    {
                        guidkey = c.Guid(nullable: false),
                        idkey = c.String(nullable: false, maxLength: 10, storeType: "nvarchar"),
                        releasedate = c.Long(nullable: false),
                        releasedategmt = c.DateTime(nullable: false, precision: 0),
                        myreleasedate = c.DateTime(nullable: false, precision: 0),
                        timemode = c.String(unicode: false),
                        currencycode = c.String(nullable: false, maxLength: 3, storeType: "nvarchar"),
                        eventname = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        eventtype = c.Int(nullable: false),
                        importance = c.String(nullable: false, maxLength: 6, storeType: "nvarchar"),
                        processed = c.Int(nullable: false),
                        actualvalue = c.String(maxLength: 20, storeType: "nvarchar"),
                        forecastvalue = c.String(maxLength: 20, storeType: "nvarchar"),
                        previousvalue = c.String(maxLength: 20, storeType: "nvarchar"),
                        oldpreviousvalue = c.String(maxLength: 20, storeType: "nvarchar"),
                        linkurl = c.String(maxLength: 2048, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.guidkey, t.idkey })
                .Index(t => t.myreleasedate)
                .Index(t => t.eventname);
            
            CreateTable(
                "dbo.IndustryCode17Master",
                c => new
                    {
                        code = c.String(nullable: false, maxLength: 2, storeType: "nvarchar"),
                        name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.code);
            
            CreateTable(
                "dbo.IndustryCode33Master",
                c => new
                    {
                        code = c.String(nullable: false, maxLength: 4, storeType: "nvarchar"),
                        name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.code);
            
            CreateTable(
                "dbo.MarketMasters",
                c => new
                    {
                        marketid = c.String(nullable: false, maxLength: 2, storeType: "nvarchar"),
                        marketname = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.marketid);
            
            CreateTable(
                "dbo.OldCandleSticks",
                c => new
                    {
                        guidkey = c.Guid(nullable: false),
                        stockcode = c.String(nullable: false, maxLength: 4, storeType: "nvarchar"),
                        businessday = c.DateTime(nullable: false, precision: 0),
                        open = c.Double(nullable: false),
                        high = c.Double(nullable: false),
                        low = c.Double(nullable: false),
                        close = c.Double(nullable: false),
                        volume = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.guidkey, t.stockcode, t.businessday });
            
            CreateTable(
                "dbo.OldStockMasters",
                c => new
                    {
                        guidkey = c.Guid(nullable: false),
                        stockcode = c.String(nullable: false, maxLength: 4, storeType: "nvarchar"),
                        deletedate = c.String(maxLength: 6, storeType: "nvarchar"),
                        stockname = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        marketcode = c.String(maxLength: 2, storeType: "nvarchar"),
                        industrycode33 = c.String(maxLength: 4, storeType: "nvarchar"),
                        industrycode17 = c.String(maxLength: 2, storeType: "nvarchar"),
                        classcode = c.String(maxLength: 2, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.guidkey, t.stockcode });
            
            CreateTable(
                "dbo.StockMasters",
                c => new
                    {
                        stockcode = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        stockname = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        marketcode = c.String(maxLength: 2, storeType: "nvarchar"),
                        industrycode33 = c.String(maxLength: 4, storeType: "nvarchar"),
                        industrycode17 = c.String(maxLength: 2, storeType: "nvarchar"),
                        classcode = c.String(maxLength: 2, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.stockcode);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.IndexCalendars", new[] { "eventname" });
            DropIndex("dbo.IndexCalendars", new[] { "myreleasedate" });
            DropTable("dbo.StockMasters");
            DropTable("dbo.OldStockMasters");
            DropTable("dbo.OldCandleSticks");
            DropTable("dbo.MarketMasters");
            DropTable("dbo.IndustryCode33Master");
            DropTable("dbo.IndustryCode17Master");
            DropTable("dbo.IndexCalendars");
            DropTable("dbo.ClassMasters");
            DropTable("dbo.CandleSticks");
        }
    }
}
