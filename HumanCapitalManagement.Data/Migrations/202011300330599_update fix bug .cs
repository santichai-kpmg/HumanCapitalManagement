namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefixbug : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TM_PInternAssessment_Activities", newName: "TM_PInternAssessment_Activitie");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TM_PInternAssessment_Activitie", newName: "TM_PInternAssessment_Activities");
        }
    }
}
