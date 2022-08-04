namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecolGroupQuestinAuditQsAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Mass_Auditing_Question", "group_question", c => c.String());
            DropColumn("dbo.TM_Candidate_MassTIF_Audit_Qst", "group_question");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_Candidate_MassTIF_Audit_Qst", "group_question", c => c.String());
            DropColumn("dbo.TM_Mass_Auditing_Question", "group_question");
        }
    }
}
