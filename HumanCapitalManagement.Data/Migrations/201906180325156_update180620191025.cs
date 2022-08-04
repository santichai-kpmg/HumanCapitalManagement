namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update180620191025 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PES_Nomination", "being_ADDirector", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PES_Nomination", "being_ADDirector");
        }
    }
}
