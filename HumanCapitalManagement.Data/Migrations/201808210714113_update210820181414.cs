namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update210820181414 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TM_Candidate_MassTIF_Audit_Qst", "answer", c => c.String(maxLength: 2000));
            AlterColumn("dbo.TM_Candidate_MassTIF_Core", "evidence", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_Candidate_MassTIF_Core", "evidence", c => c.String(maxLength: 1000));
            AlterColumn("dbo.TM_Candidate_MassTIF_Audit_Qst", "answer", c => c.String(maxLength: 1000));
        }
    }
}
