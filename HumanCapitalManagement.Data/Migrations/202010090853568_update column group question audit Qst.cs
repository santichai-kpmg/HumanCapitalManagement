namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecolumngroupquestionauditQst : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidate_MassTIF_Audit_Qst", "group_question", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Candidate_MassTIF_Audit_Qst", "group_question");
        }
    }
}
