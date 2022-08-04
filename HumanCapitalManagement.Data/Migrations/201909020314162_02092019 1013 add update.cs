namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _020920191013addupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet_Detail", "update_date", c => c.DateTime());
            AddColumn("dbo.TimeSheet_Detail", "update_user", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet_Detail", "update_user");
            DropColumn("dbo.TimeSheet_Detail", "update_date");
        }
    }
}
