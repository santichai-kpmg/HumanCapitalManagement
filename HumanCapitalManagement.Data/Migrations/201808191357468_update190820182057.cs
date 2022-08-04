namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update190820182057 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Candidate_MassTIF_Adnl_Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Additional_Answers_Id = c.Int(),
                        TM_Candidate_MassTIF_Additional_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Additional_Answers", t => t.TM_Additional_Answers_Id)
                .ForeignKey("dbo.TM_Candidate_MassTIF_Additional", t => t.TM_Candidate_MassTIF_Additional_Id)
                .Index(t => t.TM_Additional_Answers_Id)
                .Index(t => t.TM_Candidate_MassTIF_Additional_Id);
            
            AddColumn("dbo.TM_Candidate_MassTIF_Additional", "TM_Additional_Questions_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidate_MassTIF_Additional", "TM_Additional_Questions_Id");
            AddForeignKey("dbo.TM_Candidate_MassTIF_Additional", "TM_Additional_Questions_Id", "dbo.TM_Additional_Questions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidate_MassTIF_Adnl_Answer", "TM_Candidate_MassTIF_Additional_Id", "dbo.TM_Candidate_MassTIF_Additional");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Adnl_Answer", "TM_Additional_Answers_Id", "dbo.TM_Additional_Answers");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Additional", "TM_Additional_Questions_Id", "dbo.TM_Additional_Questions");
            DropIndex("dbo.TM_Candidate_MassTIF_Adnl_Answer", new[] { "TM_Candidate_MassTIF_Additional_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Adnl_Answer", new[] { "TM_Additional_Answers_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Additional", new[] { "TM_Additional_Questions_Id" });
            DropColumn("dbo.TM_Candidate_MassTIF_Additional", "TM_Additional_Questions_Id");
            DropTable("dbo.TM_Candidate_MassTIF_Adnl_Answer");
        }
    }
}
