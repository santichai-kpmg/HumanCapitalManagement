namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update200720181511 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "candidate_ProfessionalQualification", c => c.String());
            AddColumn("dbo.TM_Candidates", "prefix_th", c => c.String(maxLength: 10));
            AddColumn("dbo.TM_Candidates", "name_th", c => c.String(maxLength: 400));
            AddColumn("dbo.TM_Candidates", "candidate_CompleteInfoForOnBoard", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_BankAccountBranchName", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_BankAccountBranchNumber", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_StudentID", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_ApplyDate", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_ApplicationStatus", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_LastUpdateDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Candidates", "candidate_LastUpdateDate");
            DropColumn("dbo.TM_Candidates", "candidate_ApplicationStatus");
            DropColumn("dbo.TM_Candidates", "candidate_ApplyDate");
            DropColumn("dbo.TM_Candidates", "candidate_StudentID");
            DropColumn("dbo.TM_Candidates", "candidate_BankAccountBranchNumber");
            DropColumn("dbo.TM_Candidates", "candidate_BankAccountBranchName");
            DropColumn("dbo.TM_Candidates", "candidate_CompleteInfoForOnBoard");
            DropColumn("dbo.TM_Candidates", "name_th");
            DropColumn("dbo.TM_Candidates", "prefix_th");
            DropColumn("dbo.TM_Candidates", "candidate_ProfessionalQualification");
        }
    }
}
