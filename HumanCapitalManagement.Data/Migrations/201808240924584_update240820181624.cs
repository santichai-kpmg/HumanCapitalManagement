namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240820181624 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "HRWrapUpComment", c => c.String());
            AlterColumn("dbo.TM_Candidates", "candidate_ApplyDate", c => c.DateTime());
            AlterColumn("dbo.TM_Candidates", "candidate_LastUpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_Candidates", "candidate_LastUpdateDate", c => c.String());
            AlterColumn("dbo.TM_Candidates", "candidate_ApplyDate", c => c.String());
            DropColumn("dbo.TM_Candidates", "HRWrapUpComment");
        }
    }
}
