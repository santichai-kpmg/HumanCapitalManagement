namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _231220201357 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PR_Candidate_Mapping", "cost_no", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_PR_Candidate_Mapping", "cost_no");
        }
    }
}
