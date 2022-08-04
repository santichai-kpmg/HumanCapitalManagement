namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _220320191522 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet_Detail", "title", c => c.String(maxLength: 1000));
            AddColumn("dbo.TimeSheet_Detail", "hours", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet_Detail", "hours");
            DropColumn("dbo.TimeSheet_Detail", "title");
        }
    }
}
