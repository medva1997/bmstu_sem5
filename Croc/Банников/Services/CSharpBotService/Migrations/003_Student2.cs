namespace CROC.Education.CSharpBotService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Students");
            AddColumn("dbo.Students", "ID", c => c.Guid(nullable: false));
            // Почему-то при построении миграции автоматически identity false не поставилось
            AlterColumn("dbo.Students", "UserID", c => c.Int(nullable: false, identity: false));
            AlterColumn("dbo.Students", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "EMail", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Students", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Students");
            AlterColumn("dbo.Students", "EMail", c => c.String());
            AlterColumn("dbo.Students", "UserName", c => c.String());
            AlterColumn("dbo.Students", "UserID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Students", "ID");
            AddPrimaryKey("dbo.Students", "UserID");
        }
    }
}
