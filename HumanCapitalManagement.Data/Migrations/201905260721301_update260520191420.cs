namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update260520191420 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PES_Nomination",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        active_status = c.String(maxLength: 10),
                        comments = c.String(),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        other_roles = c.String(maxLength: 500),
                        TM_PES_NMN_Status_Id = c.Int(),
                        TM_PES_NMN_Type_Id = c.Int(),
                        PES_Nomination_Year_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PES_Nomination_Year", t => t.PES_Nomination_Year_Id, cascadeDelete: true)
                .ForeignKey("dbo.TM_PES_NMN_Status", t => t.TM_PES_NMN_Status_Id)
                .ForeignKey("dbo.TM_PES_NMN_Type", t => t.TM_PES_NMN_Type_Id)
                .Index(t => t.TM_PES_NMN_Status_Id)
                .Index(t => t.TM_PES_NMN_Type_Id)
                .Index(t => t.PES_Nomination_Year_Id);
            
            CreateTable(
                "dbo.PES_Nomination_Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        answer = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        PES_Nomination_Id = c.Int(),
                        TM_PES_NMN_Questions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PES_Nomination", t => t.PES_Nomination_Id)
                .ForeignKey("dbo.TM_PES_NMN_Questions", t => t.TM_PES_NMN_Questions_Id)
                .Index(t => t.PES_Nomination_Id)
                .Index(t => t.TM_PES_NMN_Questions_Id);
            
            CreateTable(
                "dbo.TM_PES_NMN_Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        stype = c.String(maxLength: 300),
                        stype_desc = c.String(maxLength: 3000),
                        qgroup = c.String(maxLength: 300),
                        header = c.String(maxLength: 300),
                        question = c.String(maxLength: 3000),
                        active_status = c.String(maxLength: 10),
                        questions_type = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PES_Nomination_Competencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        answer = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_PES_NMN_Competencies_Id = c.Int(),
                        TM_PES_NMN_Competencies_Rating_Id = c.Int(),
                        PES_Nomination_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PES_Nomination", t => t.PES_Nomination_Id)
                .ForeignKey("dbo.TM_PES_NMN_Competencies", t => t.TM_PES_NMN_Competencies_Id)
                .ForeignKey("dbo.TM_PES_NMN_Competencies_Rating", t => t.TM_PES_NMN_Competencies_Rating_Id)
                .Index(t => t.TM_PES_NMN_Competencies_Id)
                .Index(t => t.TM_PES_NMN_Competencies_Rating_Id)
                .Index(t => t.PES_Nomination_Id);
            
            CreateTable(
                "dbo.TM_PES_NMN_Competencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        qgroup = c.String(maxLength: 300),
                        header = c.String(maxLength: 300),
                        question = c.String(maxLength: 1000),
                        nscore = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PES_NMN_Competencies_Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        competencies_score_en = c.String(maxLength: 250),
                        competencies_score_short_en = c.String(maxLength: 250),
                        description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PES_Nomination_Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        sfile64 = c.Binary(),
                        sfileType = c.String(maxLength: 20),
                        sfile_oldname = c.String(maxLength: 250),
                        sfile_newname = c.String(maxLength: 250),
                        description = c.String(maxLength: 500),
                        PES_Nomination_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PES_Nomination", t => t.PES_Nomination_Id)
                .Index(t => t.PES_Nomination_Id);
            
            CreateTable(
                "dbo.PES_Nomination_KPIs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        how_to = c.String(),
                        final_remark = c.String(),
                        target = c.String(maxLength: 1000),
                        target_max = c.String(maxLength: 1000),
                        actual = c.String(maxLength: 1000),
                        group_actual = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        group_target = c.String(maxLength: 1000),
                        group_target_max = c.String(maxLength: 1000),
                        PES_Nomination_Id = c.Int(),
                        TM_KPIs_Base_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PES_Nomination", t => t.PES_Nomination_Id)
                .ForeignKey("dbo.TM_KPIs_Base", t => t.TM_KPIs_Base_Id)
                .Index(t => t.PES_Nomination_Id)
                .Index(t => t.TM_KPIs_Base_Id);
            
            CreateTable(
                "dbo.PES_Nomination_Signatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Req_Approve_user = c.String(maxLength: 50),
                        Approve_date = c.DateTime(),
                        Approve_user = c.String(maxLength: 50),
                        Approve_status = c.String(maxLength: 10),
                        responses = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_PES_NMN_SignatureStep_Id = c.Int(),
                        PES_Nomination_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_PES_NMN_SignatureStep", t => t.TM_PES_NMN_SignatureStep_Id)
                .ForeignKey("dbo.PES_Nomination", t => t.PES_Nomination_Id)
                .Index(t => t.TM_PES_NMN_SignatureStep_Id)
                .Index(t => t.PES_Nomination_Id);
            
            CreateTable(
                "dbo.TM_PES_NMN_SignatureStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        Step_name_en = c.String(maxLength: 250),
                        Step_short_name_en = c.String(maxLength: 250),
                        Step_description = c.String(maxLength: 500),
                        type_of_step = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PES_Nomination_Year",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        evaluation_year = c.DateTime(),
                        active_status = c.String(maxLength: 10),
                        comments = c.String(maxLength: 1500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        sfile64 = c.Binary(),
                        sfileType = c.String(maxLength: 20),
                        sfile_oldname = c.String(maxLength: 250),
                        sfile_newname = c.String(maxLength: 250),
                        actual_sfile64 = c.Binary(),
                        actual_sfileType = c.String(maxLength: 20),
                        actual_sfile_oldname = c.String(maxLength: 250),
                        actual_sfile_newname = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PES_NMN_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        status_name_en = c.String(maxLength: 250),
                        status_short_name_en = c.String(maxLength: 250),
                        status_description = c.String(maxLength: 500),
                        can_edit = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PES_NMN_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        type_name_en = c.String(maxLength: 250),
                        type_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PES_Nomination", "TM_PES_NMN_Type_Id", "dbo.TM_PES_NMN_Type");
            DropForeignKey("dbo.PES_Nomination", "TM_PES_NMN_Status_Id", "dbo.TM_PES_NMN_Status");
            DropForeignKey("dbo.PES_Nomination", "PES_Nomination_Year_Id", "dbo.PES_Nomination_Year");
            DropForeignKey("dbo.PES_Nomination_Signatures", "PES_Nomination_Id", "dbo.PES_Nomination");
            DropForeignKey("dbo.PES_Nomination_Signatures", "TM_PES_NMN_SignatureStep_Id", "dbo.TM_PES_NMN_SignatureStep");
            DropForeignKey("dbo.PES_Nomination_KPIs", "TM_KPIs_Base_Id", "dbo.TM_KPIs_Base");
            DropForeignKey("dbo.PES_Nomination_KPIs", "PES_Nomination_Id", "dbo.PES_Nomination");
            DropForeignKey("dbo.PES_Nomination_Files", "PES_Nomination_Id", "dbo.PES_Nomination");
            DropForeignKey("dbo.PES_Nomination_Competencies", "TM_PES_NMN_Competencies_Rating_Id", "dbo.TM_PES_NMN_Competencies_Rating");
            DropForeignKey("dbo.PES_Nomination_Competencies", "TM_PES_NMN_Competencies_Id", "dbo.TM_PES_NMN_Competencies");
            DropForeignKey("dbo.PES_Nomination_Competencies", "PES_Nomination_Id", "dbo.PES_Nomination");
            DropForeignKey("dbo.PES_Nomination_Answer", "TM_PES_NMN_Questions_Id", "dbo.TM_PES_NMN_Questions");
            DropForeignKey("dbo.PES_Nomination_Answer", "PES_Nomination_Id", "dbo.PES_Nomination");
            DropIndex("dbo.PES_Nomination_Signatures", new[] { "PES_Nomination_Id" });
            DropIndex("dbo.PES_Nomination_Signatures", new[] { "TM_PES_NMN_SignatureStep_Id" });
            DropIndex("dbo.PES_Nomination_KPIs", new[] { "TM_KPIs_Base_Id" });
            DropIndex("dbo.PES_Nomination_KPIs", new[] { "PES_Nomination_Id" });
            DropIndex("dbo.PES_Nomination_Files", new[] { "PES_Nomination_Id" });
            DropIndex("dbo.PES_Nomination_Competencies", new[] { "PES_Nomination_Id" });
            DropIndex("dbo.PES_Nomination_Competencies", new[] { "TM_PES_NMN_Competencies_Rating_Id" });
            DropIndex("dbo.PES_Nomination_Competencies", new[] { "TM_PES_NMN_Competencies_Id" });
            DropIndex("dbo.PES_Nomination_Answer", new[] { "TM_PES_NMN_Questions_Id" });
            DropIndex("dbo.PES_Nomination_Answer", new[] { "PES_Nomination_Id" });
            DropIndex("dbo.PES_Nomination", new[] { "PES_Nomination_Year_Id" });
            DropIndex("dbo.PES_Nomination", new[] { "TM_PES_NMN_Type_Id" });
            DropIndex("dbo.PES_Nomination", new[] { "TM_PES_NMN_Status_Id" });
            DropTable("dbo.TM_PES_NMN_Type");
            DropTable("dbo.TM_PES_NMN_Status");
            DropTable("dbo.PES_Nomination_Year");
            DropTable("dbo.TM_PES_NMN_SignatureStep");
            DropTable("dbo.PES_Nomination_Signatures");
            DropTable("dbo.PES_Nomination_KPIs");
            DropTable("dbo.PES_Nomination_Files");
            DropTable("dbo.TM_PES_NMN_Competencies_Rating");
            DropTable("dbo.TM_PES_NMN_Competencies");
            DropTable("dbo.PES_Nomination_Competencies");
            DropTable("dbo.TM_PES_NMN_Questions");
            DropTable("dbo.PES_Nomination_Answer");
            DropTable("dbo.PES_Nomination");
        }
    }
}
