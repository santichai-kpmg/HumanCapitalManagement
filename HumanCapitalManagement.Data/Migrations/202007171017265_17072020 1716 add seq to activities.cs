namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _170720201716addseqtoactivities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PreTraineeAssessment_Activities", "Seq", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_PreTraineeAssessment_Activities", "Seq");
        }
    }
}
