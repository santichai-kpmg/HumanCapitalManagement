namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16072001443addactitiestocanmap : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities");
            DropIndex("dbo.PreTraineeAssessment_Form", new[] { "TM_PreTraineeAssessment_Activities_Id" });
            AddColumn("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id", c => c.Int());
            CreateIndex("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id");
            AddForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities", "Id");
            DropColumn("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id", c => c.Int());
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities");
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "TM_PreTraineeAssessment_Activities_Id" });
            DropColumn("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id");
            CreateIndex("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id");
            AddForeignKey("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities", "Id");
        }
    }
}
