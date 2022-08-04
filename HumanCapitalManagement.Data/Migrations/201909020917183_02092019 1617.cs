namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _020920191617 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Perdiem_Transport", "review_date", c => c.DateTime());
            AddColumn("dbo.Perdiem_Transport", "review_user", c => c.String(maxLength: 50));
            AddColumn("dbo.Perdiem_Transport", "paid_date", c => c.DateTime());
            AddColumn("dbo.Perdiem_Transport", "paid_user", c => c.String(maxLength: 50));
            AddColumn("dbo.TimeSheet_Detail", "review_date", c => c.DateTime());
            AddColumn("dbo.TimeSheet_Detail", "review_user", c => c.String(maxLength: 50));
            AddColumn("dbo.TimeSheet_Detail", "paid_date", c => c.DateTime());
            AddColumn("dbo.TimeSheet_Detail", "paid_user", c => c.String(maxLength: 50));
            DropColumn("dbo.Perdiem_Transport", "update_date");
            DropColumn("dbo.Perdiem_Transport", "update_user");
            DropColumn("dbo.TimeSheet_Detail", "update_date");
            DropColumn("dbo.TimeSheet_Detail", "update_user");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TimeSheet_Detail", "update_user", c => c.String(maxLength: 50));
            AddColumn("dbo.TimeSheet_Detail", "update_date", c => c.DateTime());
            AddColumn("dbo.Perdiem_Transport", "update_user", c => c.String(maxLength: 50));
            AddColumn("dbo.Perdiem_Transport", "update_date", c => c.DateTime());
            DropColumn("dbo.TimeSheet_Detail", "paid_user");
            DropColumn("dbo.TimeSheet_Detail", "paid_date");
            DropColumn("dbo.TimeSheet_Detail", "review_user");
            DropColumn("dbo.TimeSheet_Detail", "review_date");
            DropColumn("dbo.Perdiem_Transport", "paid_user");
            DropColumn("dbo.Perdiem_Transport", "paid_date");
            DropColumn("dbo.Perdiem_Transport", "review_user");
            DropColumn("dbo.Perdiem_Transport", "review_date");
        }
    }
}
