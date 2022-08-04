namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _250420191405 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TM_PR_Candidate_Mapping", "daily_wage", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_PR_Candidate_Mapping", "daily_wage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
