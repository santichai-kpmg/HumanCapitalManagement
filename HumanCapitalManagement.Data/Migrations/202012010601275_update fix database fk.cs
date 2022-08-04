namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefixdatabasefk : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question");
            DropIndex("dbo.TM_Candidate_PIntern_Mass_Answer", new[] { "TM_PIntern_Mass_Form_Question_Id" });
            AddColumn("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Question_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Question_Id");
            AddForeignKey("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Question_Id", "dbo.TM_PIntern_Mass_Question", "Id");
            DropColumn("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Form_Question_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Form_Question_Id", c => c.Int());
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Question_Id", "dbo.TM_PIntern_Mass_Question");
            DropIndex("dbo.TM_Candidate_PIntern_Mass_Answer", new[] { "TM_PIntern_Mass_Question_Id" });
            DropColumn("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Question_Id");
            CreateIndex("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Form_Question_Id");
            AddForeignKey("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question", "Id");
        }
    }
}
