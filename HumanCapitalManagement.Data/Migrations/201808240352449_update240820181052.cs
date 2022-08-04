namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240820181052 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "AnnualLeaveDays", c => c.String());
            AlterColumn("dbo.TM_Candidates", "candidate_Age", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_Candidates", "candidate_Age", c => c.Int(nullable: false));
            DropColumn("dbo.TM_Candidates", "AnnualLeaveDays");
        }
    }
}
