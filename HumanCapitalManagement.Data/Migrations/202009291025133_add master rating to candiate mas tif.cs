namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmasterratingtocandiatemastif : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidate_MassTIF_Core", "TM_TIF_Rating_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidate_MassTIF_Core", "TM_TIF_Rating_Id");
            AddForeignKey("dbo.TM_Candidate_MassTIF_Core", "TM_TIF_Rating_Id", "dbo.TM_TIF_Rating", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidate_MassTIF_Core", "TM_TIF_Rating_Id", "dbo.TM_TIF_Rating");
            DropIndex("dbo.TM_Candidate_MassTIF_Core", new[] { "TM_TIF_Rating_Id" });
            DropColumn("dbo.TM_Candidate_MassTIF_Core", "TM_TIF_Rating_Id");
        }
    }
}
