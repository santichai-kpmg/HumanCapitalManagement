namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15072021251addactiitistoform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id", c => c.Int());
            CreateIndex("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id");
            AddForeignKey("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities");
            DropIndex("dbo.PreTraineeAssessment_Form", new[] { "TM_PreTraineeAssessment_Activities_Id" });
            DropColumn("dbo.PreTraineeAssessment_Form", "TM_PreTraineeAssessment_Activities_Id");
        }
    }
}
