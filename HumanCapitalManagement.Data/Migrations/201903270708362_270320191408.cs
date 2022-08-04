namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _270320191408 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet_Detail", "submit_status", c => c.String(maxLength: 10));
            AddColumn("dbo.TimeSheet_Detail", "approve_status", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet_Detail", "approve_status");
            DropColumn("dbo.TimeSheet_Detail", "submit_status");
        }
    }
}
