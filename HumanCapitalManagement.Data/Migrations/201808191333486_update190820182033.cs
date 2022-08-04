namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update190820182033 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_WorkExperience",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        CompanyName = c.String(),
                        JobPosition = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        TotalYearsOfWorkExperience = c.String(),
                        TypeOfRelatedToJob = c.String(),
                        active_status = c.String(maxLength: 10),
                        TM_Candidates_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidates", t => t.TM_Candidates_Id)
                .Index(t => t.TM_Candidates_Id);
            
            CreateTable(
                "dbo.TM_Candidate_MassTIF_Additional",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        other_answer = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Candidate_MassTIF_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_MassTIF", t => t.TM_Candidate_MassTIF_Id)
                .Index(t => t.TM_Candidate_MassTIF_Id);
            
            AddColumn("dbo.TM_Candidates", "candidate_officialNoteAnnoucement", c => c.String());
            AddColumn("dbo.TM_Candidates", "CPAPassedStatus", c => c.String());
            AddColumn("dbo.TM_Candidates", "CPAPassedYear", c => c.String());
            AddColumn("dbo.TM_Candidates", "CPALicenseNo", c => c.String());
            AddColumn("dbo.TM_Candidates", "RankGradeForHiring", c => c.String());
            AddColumn("dbo.TM_Candidates", "TypeOfCandidate", c => c.String());
            AddColumn("dbo.TM_Candidates", "YearOfPerformanceReview", c => c.String());
            AddColumn("dbo.TM_Candidate_MassTIF", "can_start_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidate_MassTIF_Additional", "TM_Candidate_MassTIF_Id", "dbo.TM_Candidate_MassTIF");
            DropForeignKey("dbo.TM_WorkExperience", "TM_Candidates_Id", "dbo.TM_Candidates");
            DropIndex("dbo.TM_Candidate_MassTIF_Additional", new[] { "TM_Candidate_MassTIF_Id" });
            DropIndex("dbo.TM_WorkExperience", new[] { "TM_Candidates_Id" });
            DropColumn("dbo.TM_Candidate_MassTIF", "can_start_date");
            DropColumn("dbo.TM_Candidates", "YearOfPerformanceReview");
            DropColumn("dbo.TM_Candidates", "TypeOfCandidate");
            DropColumn("dbo.TM_Candidates", "RankGradeForHiring");
            DropColumn("dbo.TM_Candidates", "CPALicenseNo");
            DropColumn("dbo.TM_Candidates", "CPAPassedYear");
            DropColumn("dbo.TM_Candidates", "CPAPassedStatus");
            DropColumn("dbo.TM_Candidates", "candidate_officialNoteAnnoucement");
            DropTable("dbo.TM_Candidate_MassTIF_Additional");
            DropTable("dbo.TM_WorkExperience");
        }
    }
}
