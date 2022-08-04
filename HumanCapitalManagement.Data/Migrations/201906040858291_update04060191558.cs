namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update04060191558 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PES_Nomination", "DEVELOPMENT_AREAS", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PES_Nomination", "DEVELOPMENT_AREAS");
        }
    }
}
