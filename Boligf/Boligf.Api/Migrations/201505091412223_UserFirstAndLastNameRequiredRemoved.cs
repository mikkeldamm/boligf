namespace Boligf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFirstAndLastNameRequiredRemoved : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Lastname", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Lastname", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false));
        }
    }
}
