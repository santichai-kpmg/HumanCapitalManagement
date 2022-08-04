namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatatableForformmass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Candidate_PIntern_Mass",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        hr_acknowledge = c.String(maxLength: 10),
                        acknowledge_date = c.DateTime(),
                        acknowledge_user = c.String(maxLength: 50),
                        submit_status = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        comments = c.String(maxLength: 1500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        confidentiality_agreement = c.String(maxLength: 10),
                        Recommended_Rank_Id = c.Int(),
                        TM_PIntern_Status_Id = c.Int(),
                        TM_PR_Candidate_Mapping_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Pool_Rank", t => t.Recommended_Rank_Id)
                .ForeignKey("dbo.TM_PIntern_Status", t => t.TM_PIntern_Status_Id)
                .ForeignKey("dbo.TM_PR_Candidate_Mapping", t => t.TM_PR_Candidate_Mapping_Id, cascadeDelete: true)
                .Index(t => t.Recommended_Rank_Id)
                .Index(t => t.TM_PIntern_Status_Id)
                .Index(t => t.TM_PR_Candidate_Mapping_Id);
            
            CreateTable(
                "dbo.TM_Candidate_PIntern_Mass_Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        answer = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Candidate_PIntern_Mass_Id = c.Int(),
                        TM_PIntern_Mass_Form_Question_Id = c.Int(),
                        TM_PIntern_Rating_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_PIntern_Mass", t => t.TM_Candidate_PIntern_Mass_Id)
                .ForeignKey("dbo.TM_PIntern_Mass_Form_Question", t => t.TM_PIntern_Mass_Form_Question_Id)
                .ForeignKey("dbo.TM_PIntern_Rating", t => t.TM_PIntern_Rating_Id)
                .Index(t => t.TM_Candidate_PIntern_Mass_Id)
                .Index(t => t.TM_PIntern_Mass_Form_Question_Id)
                .Index(t => t.TM_PIntern_Rating_Id);
            
            CreateTable(
                "dbo.TM_Candidate_PIntern_Mass_Approv",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        Req_Approve_user = c.String(maxLength: 50),
                        Approve_date = c.DateTime(),
                        Approve_user = c.String(maxLength: 50),
                        Approve_status = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Candidate_PIntern_Mass_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_PIntern_Mass", t => t.TM_Candidate_PIntern_Mass_Id)
                .Index(t => t.TM_Candidate_PIntern_Mass_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass", "TM_PIntern_Status_Id", "dbo.TM_PIntern_Status");
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass_Approv", "TM_Candidate_PIntern_Mass_Id", "dbo.TM_Candidate_PIntern_Mass");
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Rating_Id", "dbo.TM_PIntern_Rating");
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question");
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass_Answer", "TM_Candidate_PIntern_Mass_Id", "dbo.TM_Candidate_PIntern_Mass");
            DropForeignKey("dbo.TM_Candidate_PIntern_Mass", "Recommended_Rank_Id", "dbo.TM_Pool_Rank");
            DropIndex("dbo.TM_Candidate_PIntern_Mass_Approv", new[] { "TM_Candidate_PIntern_Mass_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Mass_Answer", new[] { "TM_PIntern_Rating_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Mass_Answer", new[] { "TM_PIntern_Mass_Form_Question_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Mass_Answer", new[] { "TM_Candidate_PIntern_Mass_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Mass", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Mass", new[] { "TM_PIntern_Status_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Mass", new[] { "Recommended_Rank_Id" });
            DropTable("dbo.TM_Candidate_PIntern_Mass_Approv");
            DropTable("dbo.TM_Candidate_PIntern_Mass_Answer");
            DropTable("dbo.TM_Candidate_PIntern_Mass");
        }
    }
}
