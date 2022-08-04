namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _071020192051 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Form", "End_Pro", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Form", "End_Pro");
        }
    }
}
