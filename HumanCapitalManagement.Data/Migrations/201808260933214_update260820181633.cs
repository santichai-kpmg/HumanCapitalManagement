namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update260820181633 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "CA_EN_CountryAbroad_Id", c => c.Int());
            AddColumn("dbo.TM_Candidates", "CA_TH_CountryAbroad_Id", c => c.Int());
            AddColumn("dbo.TM_Candidates", "PA_EN_CountryAbroad_Id", c => c.Int());
            AddColumn("dbo.TM_Candidates", "PA_TH_CountryAbroad_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidates", "CA_EN_CountryAbroad_Id");
            CreateIndex("dbo.TM_Candidates", "CA_TH_CountryAbroad_Id");
            CreateIndex("dbo.TM_Candidates", "PA_EN_CountryAbroad_Id");
            CreateIndex("dbo.TM_Candidates", "PA_TH_CountryAbroad_Id");
            AddForeignKey("dbo.TM_Candidates", "CA_EN_CountryAbroad_Id", "dbo.TM_Country", "Id");
            AddForeignKey("dbo.TM_Candidates", "CA_TH_CountryAbroad_Id", "dbo.TM_Country", "Id");
            AddForeignKey("dbo.TM_Candidates", "PA_EN_CountryAbroad_Id", "dbo.TM_Country", "Id");
            AddForeignKey("dbo.TM_Candidates", "PA_TH_CountryAbroad_Id", "dbo.TM_Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidates", "PA_TH_CountryAbroad_Id", "dbo.TM_Country");
            DropForeignKey("dbo.TM_Candidates", "PA_EN_CountryAbroad_Id", "dbo.TM_Country");
            DropForeignKey("dbo.TM_Candidates", "CA_TH_CountryAbroad_Id", "dbo.TM_Country");
            DropForeignKey("dbo.TM_Candidates", "CA_EN_CountryAbroad_Id", "dbo.TM_Country");
            DropIndex("dbo.TM_Candidates", new[] { "PA_TH_CountryAbroad_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "PA_EN_CountryAbroad_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "CA_TH_CountryAbroad_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "CA_EN_CountryAbroad_Id" });
            DropColumn("dbo.TM_Candidates", "PA_TH_CountryAbroad_Id");
            DropColumn("dbo.TM_Candidates", "PA_EN_CountryAbroad_Id");
            DropColumn("dbo.TM_Candidates", "CA_TH_CountryAbroad_Id");
            DropColumn("dbo.TM_Candidates", "CA_EN_CountryAbroad_Id");
        }
    }
}
