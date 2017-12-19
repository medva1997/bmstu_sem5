namespace CROC.Education.CSharpBotService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BotLogs",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserName = c.String(),
                        Name = c.String(),
                        FamilyName = c.String(),
                        UserID = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BotLogs");
        }
    }
}
