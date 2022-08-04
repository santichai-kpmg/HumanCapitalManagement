namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _100420191009 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet_Detail", "Approve_user", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet_Detail", "Approve_user");
        }
    }
}
