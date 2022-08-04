namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update05122181724 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Divisions", "default_grouphead", c => c.String(maxLength: 50));
            AddColumn("dbo.TM_Divisions", "default_practice", c => c.String(maxLength: 50));
            AddColumn("dbo.TM_Divisions", "default_ceo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Divisions", "default_ceo");
            DropColumn("dbo.TM_Divisions", "default_practice");
            DropColumn("dbo.TM_Divisions", "default_grouphead");
        }
    }
}
