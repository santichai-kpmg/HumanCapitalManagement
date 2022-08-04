namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update180720180151 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PR_Candidate_Mapping", "trainee_start", c => c.DateTime());
            AddColumn("dbo.TM_PR_Candidate_Mapping", "trainee_end", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_PR_Candidate_Mapping", "trainee_end");
            DropColumn("dbo.TM_PR_Candidate_Mapping", "trainee_start");
        }
    }
}
