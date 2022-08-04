namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update21082181518 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "CountryAbroadID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Candidates", "CountryAbroadID");
        }
    }
}
