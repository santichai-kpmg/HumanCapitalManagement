namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecolfamily : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Employee", "family_group", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Employee", "family_group");
        }
    }
}
