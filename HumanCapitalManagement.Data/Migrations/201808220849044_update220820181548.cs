namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update220820181548 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "CountryOfBirth_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidates", "CountryOfBirth_Id");
            AddForeignKey("dbo.TM_Candidates", "CountryOfBirth_Id", "dbo.TM_Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidates", "CountryOfBirth_Id", "dbo.TM_Country");
            DropIndex("dbo.TM_Candidates", new[] { "CountryOfBirth_Id" });
            DropColumn("dbo.TM_Candidates", "CountryOfBirth_Id");
        }
    }
}
