namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update05120181812 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Divisions", "approve_description", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Divisions", "approve_description");
        }
    }
}
