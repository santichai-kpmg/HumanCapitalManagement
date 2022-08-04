namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14072201020addcanmaptopreetraineeform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id", c => c.Int());
            CreateIndex("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id");
            AddForeignKey("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropIndex("dbo.PreTraineeAssessment_Form", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropColumn("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id");
        }
    }
}
