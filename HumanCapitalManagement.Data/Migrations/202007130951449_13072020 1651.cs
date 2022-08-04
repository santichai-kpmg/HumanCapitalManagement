namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _130720201651 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PreTraineeAssessment_Rating", newName: "TM_PreTraineeAssessment_Rating");
            RenameTable(name: "dbo.TM_Activities", newName: "TM_PreTraineeAssessment_Activities");
            AlterColumn("dbo.TM_PreTraineeAssessment_Group_Question", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_PreTraineeAssessment_Group_Question", "Name", c => c.String(maxLength: 50));
            RenameTable(name: "dbo.TM_PreTraineeAssessment_Activities", newName: "TM_Activities");
            RenameTable(name: "dbo.TM_PreTraineeAssessment_Rating", newName: "PreTraineeAssessment_Rating");
        }
    }
}
