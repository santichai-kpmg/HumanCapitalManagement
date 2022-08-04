namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _220420191151 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PR_Candidate_Mapping", "daily_wage", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_PR_Candidate_Mapping", "daily_wage");
        }
    }
}
