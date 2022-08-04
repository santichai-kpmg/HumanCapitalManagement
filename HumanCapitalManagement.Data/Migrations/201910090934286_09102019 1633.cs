namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _091020191633 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Form", "Send_Mail_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Form", "Send_Mail_Date");
        }
    }
}
