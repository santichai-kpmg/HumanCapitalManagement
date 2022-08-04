namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _150720200947addseqtorating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PreTraineeAssessment_Rating", "Seq", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_PreTraineeAssessment_Rating", "Seq");
        }
    }
}
