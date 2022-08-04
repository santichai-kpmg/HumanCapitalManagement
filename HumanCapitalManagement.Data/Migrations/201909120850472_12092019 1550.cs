namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _120920191550 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Probation_Question", "icon", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Probation_Question", "icon");
        }
    }
}
