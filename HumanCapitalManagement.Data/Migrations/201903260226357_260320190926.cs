namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _260320190926 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeSheet_Detail", "hours", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeSheet_Detail", "hours", c => c.Int());
        }
    }
}
