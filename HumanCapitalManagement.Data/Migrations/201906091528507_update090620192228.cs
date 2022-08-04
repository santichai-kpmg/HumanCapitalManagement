namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update090620192228 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Perdiem_Transport", "TM_PR_Candidate_Mapping_Id", c => c.Int());
            //CreateIndex("dbo.Perdiem_Transport", "TM_PR_Candidate_Mapping_Id");
            //AddForeignKey("dbo.Perdiem_Transport", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Perdiem_Transport", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropIndex("dbo.Perdiem_Transport", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropColumn("dbo.Perdiem_Transport", "TM_PR_Candidate_Mapping_Id");
        }
    }
}
