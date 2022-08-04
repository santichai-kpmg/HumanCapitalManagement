namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240820180944 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "AllTotalYearsOfWorkExp", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_Candidates", "CurrentOrLatestIndustry", c => c.String());
            AddColumn("dbo.TM_Candidates", "CurrentOrLatestCompanyName", c => c.String());
            AddColumn("dbo.TM_Candidates", "CurrentOrLatestPositionName", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_Age", c => c.Int(nullable: false));
            AddColumn("dbo.TM_Candidates", "CurrentOrLatestDegree_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidates", "CurrentOrLatestDegree_Id");
            AddForeignKey("dbo.TM_Candidates", "CurrentOrLatestDegree_Id", "dbo.TM_Education_Degree", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidates", "CurrentOrLatestDegree_Id", "dbo.TM_Education_Degree");
            DropIndex("dbo.TM_Candidates", new[] { "CurrentOrLatestDegree_Id" });
            DropColumn("dbo.TM_Candidates", "CurrentOrLatestDegree_Id");
            DropColumn("dbo.TM_Candidates", "candidate_Age");
            DropColumn("dbo.TM_Candidates", "CurrentOrLatestPositionName");
            DropColumn("dbo.TM_Candidates", "CurrentOrLatestCompanyName");
            DropColumn("dbo.TM_Candidates", "CurrentOrLatestIndustry");
            DropColumn("dbo.TM_Candidates", "AllTotalYearsOfWorkExp");
        }
    }
}
