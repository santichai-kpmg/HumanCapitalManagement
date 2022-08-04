namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240820181455 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "candidate_PositionAllowanceTHB", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Candidates", "candidate_PositionAllowanceTHB");
        }
    }
}
