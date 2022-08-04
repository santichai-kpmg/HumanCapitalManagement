namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _200920191037 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Detail", "add", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Detail", "add");
        }
    }
}
