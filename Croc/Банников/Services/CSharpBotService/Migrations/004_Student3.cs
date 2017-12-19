namespace CROC.Education.CSharpBotService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Students", "UserName", c => c.String());
            CreateIndex("dbo.Students", "UserID", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", new[] { "UserID" });
            AlterColumn("dbo.Students", "UserName", c => c.String(nullable: false));
            DropColumn("dbo.Students", "PhoneNumber");
        }
    }
}
