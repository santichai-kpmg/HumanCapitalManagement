namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update030820181404 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "candidate_PAHouseNo_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_PAVillageNoAndAlley_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_PAStreet_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_PAPostalCode_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_PATelephoneNumber_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_PAMobileNumber_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_CAHouseNo_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_CAVillageNoAndAlley_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_CAStreet_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_CAPostalCode_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_CATelephoneNumber_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_CAMobileNumber_EN", c => c.String());
            AddColumn("dbo.TM_Candidates", "CA_EN_TM_SubDistrict_Id", c => c.Int());
            AddColumn("dbo.TM_Candidates", "PA_EN_TM_SubDistrict_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidates", "CA_EN_TM_SubDistrict_Id");
            CreateIndex("dbo.TM_Candidates", "PA_EN_TM_SubDistrict_Id");
            AddForeignKey("dbo.TM_Candidates", "CA_EN_TM_SubDistrict_Id", "dbo.TM_SubDistrict", "Id");
            AddForeignKey("dbo.TM_Candidates", "PA_EN_TM_SubDistrict_Id", "dbo.TM_SubDistrict", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidates", "PA_EN_TM_SubDistrict_Id", "dbo.TM_SubDistrict");
            DropForeignKey("dbo.TM_Candidates", "CA_EN_TM_SubDistrict_Id", "dbo.TM_SubDistrict");
            DropIndex("dbo.TM_Candidates", new[] { "PA_EN_TM_SubDistrict_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "CA_EN_TM_SubDistrict_Id" });
            DropColumn("dbo.TM_Candidates", "PA_EN_TM_SubDistrict_Id");
            DropColumn("dbo.TM_Candidates", "CA_EN_TM_SubDistrict_Id");
            DropColumn("dbo.TM_Candidates", "candidate_CAMobileNumber_EN");
            DropColumn("dbo.TM_Candidates", "candidate_CATelephoneNumber_EN");
            DropColumn("dbo.TM_Candidates", "candidate_CAPostalCode_EN");
            DropColumn("dbo.TM_Candidates", "candidate_CAStreet_EN");
            DropColumn("dbo.TM_Candidates", "candidate_CAVillageNoAndAlley_EN");
            DropColumn("dbo.TM_Candidates", "candidate_CAHouseNo_EN");
            DropColumn("dbo.TM_Candidates", "candidate_PAMobileNumber_EN");
            DropColumn("dbo.TM_Candidates", "candidate_PATelephoneNumber_EN");
            DropColumn("dbo.TM_Candidates", "candidate_PAPostalCode_EN");
            DropColumn("dbo.TM_Candidates", "candidate_PAStreet_EN");
            DropColumn("dbo.TM_Candidates", "candidate_PAVillageNoAndAlley_EN");
            DropColumn("dbo.TM_Candidates", "candidate_PAHouseNo_EN");
        }
    }
}
