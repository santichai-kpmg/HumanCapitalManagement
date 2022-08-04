namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14072021029updatcolumninpretraineeform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PreTraineeAssessment_Form", "First_No", c => c.String(maxLength: 10));
            AddColumn("dbo.PreTraineeAssessment_Form", "First_Submit_Date", c => c.DateTime());
            AddColumn("dbo.PreTraineeAssessment_Form", "Second_No", c => c.String(maxLength: 10));
            AddColumn("dbo.PreTraineeAssessment_Form", "Second_Submit_Date", c => c.DateTime());
            DropColumn("dbo.PreTraineeAssessment_Form", "PM_No");
            DropColumn("dbo.PreTraineeAssessment_Form", "PM_Submit_Date");
            DropColumn("dbo.PreTraineeAssessment_Form", "GroupHead_No");
            DropColumn("dbo.PreTraineeAssessment_Form", "GroupHead_Submit_Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PreTraineeAssessment_Form", "GroupHead_Submit_Date", c => c.DateTime());
            AddColumn("dbo.PreTraineeAssessment_Form", "GroupHead_No", c => c.String(maxLength: 10));
            AddColumn("dbo.PreTraineeAssessment_Form", "PM_Submit_Date", c => c.DateTime());
            AddColumn("dbo.PreTraineeAssessment_Form", "PM_No", c => c.String(maxLength: 10));
            DropColumn("dbo.PreTraineeAssessment_Form", "Second_Submit_Date");
            DropColumn("dbo.PreTraineeAssessment_Form", "Second_No");
            DropColumn("dbo.PreTraineeAssessment_Form", "First_Submit_Date");
            DropColumn("dbo.PreTraineeAssessment_Form", "First_No");
        }
    }
}
