namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _241220201318 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PR_Candidate_Mapping", "TM_Divisions_Id", c => c.Int());
            CreateIndex("dbo.TM_PR_Candidate_Mapping", "TM_Divisions_Id");
            AddForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_Divisions_Id", "dbo.TM_Divisions", "Id");
            DropColumn("dbo.TM_PR_Candidate_Mapping", "cost_no");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_PR_Candidate_Mapping", "cost_no", c => c.String(maxLength: 50));
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_Divisions_Id", "dbo.TM_Divisions");
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "TM_Divisions_Id" });
            DropColumn("dbo.TM_PR_Candidate_Mapping", "TM_Divisions_Id");
        }
    }
}
