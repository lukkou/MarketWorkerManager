namespace MarketWorkerManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlgTableAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationFlgs",
                c => new
                    {
                        guidkey = c.Guid(nullable: false),
                        idkey = c.String(nullable: false, maxLength: 10, storeType: "nvarchar"),
                        tweetflg = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.guidkey, t.idkey });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NotificationFlgs");
        }
    }
}
