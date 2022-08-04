namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update110320191329 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeSheet_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        date_start = c.DateTime(),
                        date_end = c.DateTime(),
                        Engagement_Code = c.String(maxLength: 50),
                        remark = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        trainee_create_date = c.DateTime(),
                        trainee_create_user = c.Int(),
                        trainee_update_date = c.DateTime(),
                        trainee_update_user = c.Int(),
                        TM_Time_Type_Id = c.Int(),
                        TimeSheet_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeSheet_Form", t => t.TimeSheet_Form_Id)
                .ForeignKey("dbo.TM_Time_Type", t => t.TM_Time_Type_Id)
                .Index(t => t.TM_Time_Type_Id)
                .Index(t => t.TimeSheet_Form_Id);
            
            CreateTable(
                "dbo.TimeSheet_Form",
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
                        TM_TimeSheet_Status_Id = c.Int(),
                        req_Approve_user = c.String(maxLength: 50),
                        Approve_date = c.DateTime(),
                        Approve_user = c.String(maxLength: 50),
                        Approve_status = c.String(maxLength: 10),
                        Approve_remark = c.String(maxLength: 500),
                        trainee_create_date = c.DateTime(),
                        trainee_create_user = c.Int(),
                        trainee_update_date = c.DateTime(),
                        trainee_update_user = c.Int(),
                        TM_PR_Candidate_Mapping_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_PR_Candidate_Mapping", t => t.TM_PR_Candidate_Mapping_Id)
                .ForeignKey("dbo.TM_TimeSheet_Status", t => t.TM_TimeSheet_Status_Id)
                .Index(t => t.TM_TimeSheet_Status_Id)
                .Index(t => t.TM_PR_Candidate_Mapping_Id);
            
            CreateTable(
                "dbo.TM_TimeSheet_Status",
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
                "dbo.TM_Time_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        type_name_en = c.String(maxLength: 250),
                        type_short_name_en = c.String(maxLength: 250),
                        type_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_Candidates", "trainee_email", c => c.String(maxLength: 250));
            AddColumn("dbo.TM_Candidates", "is_verify", c => c.String(maxLength: 10));
            AddColumn("dbo.TM_Candidates", "verify_code", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSheet_Detail", "TM_Time_Type_Id", "dbo.TM_Time_Type");
            DropForeignKey("dbo.TimeSheet_Form", "TM_TimeSheet_Status_Id", "dbo.TM_TimeSheet_Status");
            DropForeignKey("dbo.TimeSheet_Form", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.TimeSheet_Detail", "TimeSheet_Form_Id", "dbo.TimeSheet_Form");
            DropIndex("dbo.TimeSheet_Form", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TimeSheet_Form", new[] { "TM_TimeSheet_Status_Id" });
            DropIndex("dbo.TimeSheet_Detail", new[] { "TimeSheet_Form_Id" });
            DropIndex("dbo.TimeSheet_Detail", new[] { "TM_Time_Type_Id" });
            DropColumn("dbo.TM_Candidates", "verify_code");
            DropColumn("dbo.TM_Candidates", "is_verify");
            DropColumn("dbo.TM_Candidates", "trainee_email");
            DropTable("dbo.TM_Time_Type");
            DropTable("dbo.TM_TimeSheet_Status");
            DropTable("dbo.TimeSheet_Form");
            DropTable("dbo.TimeSheet_Detail");
        }
    }
}
