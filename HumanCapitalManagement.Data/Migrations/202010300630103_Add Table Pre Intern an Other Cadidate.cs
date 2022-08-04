namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablePreInternanOtherCadidate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Candidate_PIntern",
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
                "dbo.TM_Candidate_PIntern_Answer",
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
                        TM_Candidate_PIntern_Id = c.Int(),
                        TM_PIntern_Form_Question_Id = c.Int(),
                        TM_PIntern_Rating_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_PIntern", t => t.TM_Candidate_PIntern_Id)
                .ForeignKey("dbo.TM_PIntern_Form_Question", t => t.TM_PIntern_Form_Question_Id)
                .ForeignKey("dbo.TM_PIntern_Rating", t => t.TM_PIntern_Rating_Id)
                .Index(t => t.TM_Candidate_PIntern_Id)
                .Index(t => t.TM_PIntern_Form_Question_Id)
                .Index(t => t.TM_PIntern_Rating_Id);
            
            CreateTable(
                "dbo.TM_PIntern_Form_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        header = c.String(maxLength: 300),
                        question = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_PIntern_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_PIntern_Form", t => t.TM_PIntern_Form_Id)
                .Index(t => t.TM_PIntern_Form_Id);
            
            CreateTable(
                "dbo.TM_PIntern_Form",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        active_status = c.String(maxLength: 10),
                        action_date = c.DateTime(),
                        description = c.String(maxLength: 500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PIntern_Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        rating_name_th = c.String(maxLength: 250),
                        rating_name_en = c.String(maxLength: 250),
                        rating_description = c.String(maxLength: 500),
                        point = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Candidate_PIntern_Approv",
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
                        TM_Candidate_PIntern_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_PIntern", t => t.TM_Candidate_PIntern_Id)
                .Index(t => t.TM_Candidate_PIntern_Id);
            
            CreateTable(
                "dbo.TM_PIntern_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        PIntern_status_name_th = c.String(maxLength: 250),
                        PIntern_status_name_en = c.String(maxLength: 250),
                        PIntern_short_name_en = c.String(maxLength: 250),
                        PIntern_status_description = c.String(maxLength: 500),
                        status_id = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PInternAssessment_Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Activities_name_en = c.String(maxLength: 250),
                        Activities_name_th = c.String(maxLength: 250),
                        Activities_short_name_en = c.String(maxLength: 250),
                        Activities_short_name_th = c.String(maxLength: 250),
                        Activities_descriptions = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        Seq = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidate_PIntern", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.TM_Candidate_PIntern", "TM_PIntern_Status_Id", "dbo.TM_PIntern_Status");
            DropForeignKey("dbo.TM_Candidate_PIntern_Approv", "TM_Candidate_PIntern_Id", "dbo.TM_Candidate_PIntern");
            DropForeignKey("dbo.TM_Candidate_PIntern_Answer", "TM_PIntern_Rating_Id", "dbo.TM_PIntern_Rating");
            DropForeignKey("dbo.TM_Candidate_PIntern_Answer", "TM_PIntern_Form_Question_Id", "dbo.TM_PIntern_Form_Question");
            DropForeignKey("dbo.TM_PIntern_Form_Question", "TM_PIntern_Form_Id", "dbo.TM_PIntern_Form");
            DropForeignKey("dbo.TM_Candidate_PIntern_Answer", "TM_Candidate_PIntern_Id", "dbo.TM_Candidate_PIntern");
            DropForeignKey("dbo.TM_Candidate_PIntern", "Recommended_Rank_Id", "dbo.TM_Pool_Rank");
            DropIndex("dbo.TM_Candidate_PIntern_Approv", new[] { "TM_Candidate_PIntern_Id" });
            DropIndex("dbo.TM_PIntern_Form_Question", new[] { "TM_PIntern_Form_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Answer", new[] { "TM_PIntern_Rating_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Answer", new[] { "TM_PIntern_Form_Question_Id" });
            DropIndex("dbo.TM_Candidate_PIntern_Answer", new[] { "TM_Candidate_PIntern_Id" });
            DropIndex("dbo.TM_Candidate_PIntern", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TM_Candidate_PIntern", new[] { "TM_PIntern_Status_Id" });
            DropIndex("dbo.TM_Candidate_PIntern", new[] { "Recommended_Rank_Id" });
            DropTable("dbo.TM_PInternAssessment_Activities");
            DropTable("dbo.TM_PIntern_Status");
            DropTable("dbo.TM_Candidate_PIntern_Approv");
            DropTable("dbo.TM_PIntern_Rating");
            DropTable("dbo.TM_PIntern_Form");
            DropTable("dbo.TM_PIntern_Form_Question");
            DropTable("dbo.TM_Candidate_PIntern_Answer");
            DropTable("dbo.TM_Candidate_PIntern");
        }
    }
}
