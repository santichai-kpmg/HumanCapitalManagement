namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _310720191119 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Perdiem_Transport", "submit_date", c => c.DateTime());
            AddColumn("dbo.Perdiem_Transport", "approve_date", c => c.DateTime());
            AddColumn("dbo.TimeSheet_Detail", "submit_date", c => c.DateTime());
            AddColumn("dbo.TimeSheet_Detail", "approve_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet_Detail", "approve_date");
            DropColumn("dbo.TimeSheet_Detail", "submit_date");
            DropColumn("dbo.Perdiem_Transport", "approve_date");
            DropColumn("dbo.Perdiem_Transport", "submit_date");
        }
    }
}
