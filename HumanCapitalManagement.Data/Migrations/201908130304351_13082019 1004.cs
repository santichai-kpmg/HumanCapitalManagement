namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _130820191004 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "Given_Date", c => c.DateTime());
            AddColumn("dbo.Feedbacks", "Request_Date", c => c.DateTime());
            AddColumn("dbo.Feedbacks", "Approve_User", c => c.String(maxLength: 50));
            AddColumn("dbo.Feedbacks", "Approve_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "Approve_Date");
            DropColumn("dbo.Feedbacks", "Approve_User");
            DropColumn("dbo.Feedbacks", "Request_Date");
            DropColumn("dbo.Feedbacks", "Given_Date");
        }
    }
}
