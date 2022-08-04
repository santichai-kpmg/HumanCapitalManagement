namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TBmst_Division",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BU = c.String(),
                        DivisionName = c.String(),
                        devision_code = c.String(),
                        Sort = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TBmst_SubTeam",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubDivision = c.String(),
                        CreateBy = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        Division_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TBmst_Division", t => t.Division_Id, cascadeDelete: true)
                .Index(t => t.Division_Id);
            
            CreateTable(
                "dbo.TBmst_SubTeamMember",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmpNo = c.String(),
                        Rank = c.String(),
                        CreateBy = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        SubTeam_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TBmst_SubTeam", t => t.SubTeam_Id, cascadeDelete: true)
                .Index(t => t.SubTeam_Id);
            
            CreateTable(
                "dbo.E_Mail_History",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        sent_status = c.String(maxLength: 10),
                        descriptions = c.String(),
                        mail_to = c.String(),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        MailContent_Id = c.Int(nullable: false),
                        PersonnelRequest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailContents", t => t.MailContent_Id, cascadeDelete: true)
                .ForeignKey("dbo.PersonnelRequests", t => t.PersonnelRequest_Id, cascadeDelete: true)
                .Index(t => t.MailContent_Id)
                .Index(t => t.PersonnelRequest_Id);
            
            CreateTable(
                "dbo.MailContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mail_type = c.String(maxLength: 50),
                        mail_type_name = c.String(maxLength: 250),
                        mail_header = c.String(maxLength: 500),
                        sender_name = c.String(maxLength: 500),
                        content = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_MailContent_Cc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        description = c.String(maxLength: 500),
                        MailContent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailContents", t => t.MailContent_Id)
                .Index(t => t.MailContent_Id);
            
            CreateTable(
                "dbo.TM_MailContent_Cc_bymail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        e_mail = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        description = c.String(maxLength: 500),
                        MailContent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailContents", t => t.MailContent_Id)
                .Index(t => t.MailContent_Id);
            
            CreateTable(
                "dbo.TM_MailContent_Param",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        param = c.String(maxLength: 150),
                        description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        MailContent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailContents", t => t.MailContent_Id)
                .Index(t => t.MailContent_Id);
            
            CreateTable(
                "dbo.PersonnelRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefNo = c.String(maxLength: 50),
                        user_replaced = c.String(maxLength: 50),
                        replaced_status = c.String(maxLength: 10),
                        no_of_headcount = c.Int(nullable: false),
                        target_period = c.DateTime(),
                        target_period_to = c.DateTime(),
                        job_descriptions = c.String(),
                        qualification_experience = c.String(),
                        remark = c.String(),
                        request_date = c.DateTime(),
                        request_user = c.String(maxLength: 50),
                        Req_BUApprove_user = c.String(maxLength: 50),
                        BUApprove_date = c.DateTime(),
                        BUApprove_user = c.String(maxLength: 50),
                        BUApprove_status = c.String(maxLength: 10),
                        BUApprove_remark = c.String(maxLength: 500),
                        Req_HeadApprove_user = c.String(maxLength: 50),
                        HeadApprove_date = c.DateTime(),
                        HeadApprove_user = c.String(maxLength: 50),
                        HeadApprove_status = c.String(maxLength: 10),
                        HeadApprove_remark = c.String(maxLength: 500),
                        Req_CeoApprove_user = c.String(maxLength: 50),
                        CeoApprove_date = c.DateTime(),
                        CeoApprove_user = c.String(maxLength: 50),
                        CeoApprove_status = c.String(maxLength: 10),
                        CeoApprove_remark = c.String(maxLength: 500),
                        need_ceo_approve = c.String(maxLength: 10),
                        reject_reason = c.String(maxLength: 1000),
                        reject_user = c.String(maxLength: 50),
                        cancel_reason = c.String(maxLength: 1000),
                        cancel_user = c.String(maxLength: 50),
                        type_of_TIFForm = c.String(maxLength: 10),
                        hr_remark = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        no_of_eva = c.Int(),
                        TM_Divisions_Id = c.Int(),
                        TM_Employment_Request_Id = c.Int(),
                        TM_Pool_Rank_Id = c.Int(),
                        TM_Position_Id = c.Int(),
                        TM_PR_Status_Id = c.Int(),
                        TM_SubGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Divisions", t => t.TM_Divisions_Id)
                .ForeignKey("dbo.TM_Employment_Request", t => t.TM_Employment_Request_Id)
                .ForeignKey("dbo.TM_Pool_Rank", t => t.TM_Pool_Rank_Id)
                .ForeignKey("dbo.TM_Position", t => t.TM_Position_Id)
                .ForeignKey("dbo.TM_PR_Status", t => t.TM_PR_Status_Id)
                .ForeignKey("dbo.TM_SubGroup", t => t.TM_SubGroup_Id)
                .Index(t => t.TM_Divisions_Id)
                .Index(t => t.TM_Employment_Request_Id)
                .Index(t => t.TM_Pool_Rank_Id)
                .Index(t => t.TM_Position_Id)
                .Index(t => t.TM_PR_Status_Id)
                .Index(t => t.TM_SubGroup_Id);
            
            CreateTable(
                "dbo.TM_Divisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        division_code = c.String(maxLength: 50),
                        division_name_th = c.String(maxLength: 250),
                        division_name_en = c.String(maxLength: 250),
                        division_short_name_th = c.String(maxLength: 250),
                        division_short_name_en = c.String(maxLength: 250),
                        division_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                        TM_Pool_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Pool", t => t.TM_Pool_Id)
                .Index(t => t.TM_Pool_Id);
            
            CreateTable(
                "dbo.TM_Pool",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pool_code = c.String(maxLength: 50),
                        Pool_name_th = c.String(maxLength: 250),
                        Pool_name_en = c.String(maxLength: 250),
                        Pool_short_name_th = c.String(maxLength: 250),
                        Pool_short_name_en = c.String(maxLength: 250),
                        Pool_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                        TM_Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Company", t => t.TM_Company_Id)
                .Index(t => t.TM_Company_Id);
            
            CreateTable(
                "dbo.TM_Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_code = c.String(maxLength: 50),
                        Company_name_th = c.String(maxLength: 250),
                        Company_name_en = c.String(maxLength: 250),
                        Company_short_name_th = c.String(maxLength: 250),
                        Company_short_name_en = c.String(maxLength: 250),
                        Company_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Company_Approve_Permit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        description = c.String(maxLength: 500),
                        TM_Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Company", t => t.TM_Company_Id)
                .Index(t => t.TM_Company_Id);
            
            CreateTable(
                "dbo.TM_Pool_Approve_Permit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        description = c.String(maxLength: 500),
                        TM_Pool_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Pool", t => t.TM_Pool_Id)
                .Index(t => t.TM_Pool_Id);
            
            CreateTable(
                "dbo.TM_Pool_Rank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pool_rank_name_th = c.String(maxLength: 250),
                        Pool_rank_name_en = c.String(maxLength: 250),
                        Pool_rank_short_name_th = c.String(maxLength: 250),
                        Pool_rank_short_name_en = c.String(maxLength: 250),
                        Pool_rank_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Pool_Id = c.Int(),
                        TM_Rank_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Pool", t => t.TM_Pool_Id)
                .ForeignKey("dbo.TM_Rank", t => t.TM_Rank_Id)
                .Index(t => t.TM_Pool_Id)
                .Index(t => t.TM_Rank_Id);
            
            CreateTable(
                "dbo.TM_Rank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        rank_name_th = c.String(maxLength: 250),
                        rank_name_en = c.String(maxLength: 250),
                        rank_short_name_th = c.String(maxLength: 250),
                        rank_short_name_en = c.String(maxLength: 250),
                        rank_description = c.String(maxLength: 500),
                        ceo_approve = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        piority = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Position",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        position_name_en = c.String(maxLength: 250),
                        position_name_th = c.String(maxLength: 250),
                        position_short_name_en = c.String(maxLength: 250),
                        position_short_name_th = c.String(maxLength: 250),
                        job_descriptions = c.String(),
                        qualification_experience = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Divisions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Divisions", t => t.TM_Divisions_Id)
                .Index(t => t.TM_Divisions_Id);
            
            CreateTable(
                "dbo.TM_SubGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        head_user_no = c.String(maxLength: 50),
                        head_user_id = c.String(maxLength: 50),
                        sub_group_code = c.String(maxLength: 50),
                        sub_group_name_th = c.String(maxLength: 250),
                        sub_group_name_en = c.String(maxLength: 250),
                        sub_group_short_name_th = c.String(maxLength: 250),
                        sub_group_short_name_en = c.String(maxLength: 250),
                        sub_group_description = c.String(maxLength: 500),
                        seq = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Divisions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Divisions", t => t.TM_Divisions_Id)
                .Index(t => t.TM_Divisions_Id);
            
            CreateTable(
                "dbo.TM_UnitGroup_Approve_Permit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        description = c.String(maxLength: 500),
                        TM_Divisions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Divisions", t => t.TM_Divisions_Id)
                .Index(t => t.TM_Divisions_Id);
            
            CreateTable(
                "dbo.TM_Employment_Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Employment_Type_Id = c.Int(),
                        TM_Request_Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Employment_Type", t => t.TM_Employment_Type_Id)
                .ForeignKey("dbo.TM_Request_Type", t => t.TM_Request_Type_Id)
                .Index(t => t.TM_Employment_Type_Id)
                .Index(t => t.TM_Request_Type_Id);
            
            CreateTable(
                "dbo.TM_Employment_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        employee_type_name_th = c.String(maxLength: 250),
                        employee_type_name_en = c.String(maxLength: 250),
                        employee_type_description = c.String(maxLength: 500),
                        target_period_validate = c.String(maxLength: 10),
                        personnel_type = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Request_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        replaced_user = c.String(maxLength: 10),
                        request_type_name_en = c.String(maxLength: 250),
                        request_type_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PR_Candidate_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ntime = c.Int(nullable: false),
                        seq = c.Int(),
                        description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        PersonnelRequest_Id = c.Int(),
                        TM_Candidate_Rank_Id = c.Int(),
                        TM_Candidates_Id = c.Int(),
                        TM_Recruitment_Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonnelRequests", t => t.PersonnelRequest_Id)
                .ForeignKey("dbo.TM_Candidate_Rank", t => t.TM_Candidate_Rank_Id)
                .ForeignKey("dbo.TM_Candidates", t => t.TM_Candidates_Id)
                .ForeignKey("dbo.TM_Recruitment_Team", t => t.TM_Recruitment_Team_Id)
                .Index(t => t.PersonnelRequest_Id)
                .Index(t => t.TM_Candidate_Rank_Id)
                .Index(t => t.TM_Candidates_Id)
                .Index(t => t.TM_Recruitment_Team_Id);
            
            CreateTable(
                "dbo.TM_Candidate_Rank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        piority = c.Int(),
                        crank_name_th = c.String(maxLength: 250),
                        crank_name_en = c.String(maxLength: 250),
                        crank_short_name_th = c.String(maxLength: 250),
                        crank_short_name_en = c.String(maxLength: 250),
                        crank_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Candidate_Status_Cycle",
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
                        TM_Candidate_Status_Id = c.Int(),
                        TM_PR_Candidate_Mapping_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_Status", t => t.TM_Candidate_Status_Id)
                .ForeignKey("dbo.TM_PR_Candidate_Mapping", t => t.TM_PR_Candidate_Mapping_Id)
                .Index(t => t.TM_Candidate_Status_Id)
                .Index(t => t.TM_PR_Candidate_Mapping_Id);
            
            CreateTable(
                "dbo.TM_Candidate_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        candidate_status_name_th = c.String(maxLength: 250),
                        candidate_status_name_en = c.String(maxLength: 250),
                        candidate_status_description = c.String(maxLength: 500),
                        remark_validate = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Candidate_Status_Next",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        next_status_id = c.Int(nullable: false),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Candidate_Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_Status", t => t.TM_Candidate_Status_Id)
                .Index(t => t.TM_Candidate_Status_Id);
            
            CreateTable(
                "dbo.TM_Candidates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        id_card = c.String(maxLength: 20),
                        first_name_en = c.String(maxLength: 250),
                        last_name_en = c.String(maxLength: 250),
                        first_name_th = c.String(maxLength: 250),
                        last_name_th = c.String(maxLength: 250),
                        active_status = c.String(maxLength: 10),
                        save_success = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        candidate_phone = c.String(maxLength: 50),
                        candidate_oxfordscore = c.Decimal(precision: 18, scale: 2),
                        candidate_TotalYearsOfWorkRelatedToThisPosition = c.Decimal(precision: 18, scale: 2),
                        candidate_TotalYearsOfWorkNotRelatedToThisPosition = c.Decimal(precision: 18, scale: 2),
                        candidate_BaseSalary = c.Decimal(precision: 18, scale: 2),
                        candidate_NMGTestScore = c.Decimal(precision: 18, scale: 2),
                        candidate_NMGTestDate = c.DateTime(),
                        candidate_CurrentIndustry = c.String(),
                        candidate_CurrentPositionName = c.String(),
                        candidate_MobileAllowance = c.Decimal(precision: 18, scale: 2),
                        candidate_TransportationAllowance = c.Decimal(precision: 18, scale: 2),
                        candidate_OtherAllowance = c.Decimal(precision: 18, scale: 2),
                        candidate_AnnualLeave = c.Decimal(precision: 18, scale: 2),
                        candidate_VariableBonus = c.Decimal(precision: 18, scale: 2),
                        candidate_FixedBonus = c.Decimal(precision: 18, scale: 2),
                        candidate_ExpectedSalary = c.Decimal(precision: 18, scale: 2),
                        candidate_user_id = c.String(maxLength: 50),
                        candidate_Company = c.String(),
                        candidate_OrgUnit = c.String(),
                        candidate_CostCenter = c.String(),
                        candidate_Position = c.String(),
                        candidate_TypeOfEmployment = c.String(),
                        candidate_Prefix = c.String(),
                        candidate_NickName = c.String(),
                        candidate_AlternativeNameTH = c.String(),
                        candidate_Gender = c.String(),
                        candidate_DateOfBirth = c.DateTime(),
                        candidate_BirthPlace = c.String(),
                        candidate_CountryOfBirth = c.String(),
                        candidate_MaritalStatus = c.String(),
                        candidate_Nationality = c.String(),
                        candidate_PAHouseNo = c.String(),
                        candidate_PAMooAndSoi = c.String(),
                        candidate_PAStreet = c.String(),
                        candidate_PAPostalCode = c.String(),
                        candidate_PATelephoneNumber = c.String(),
                        candidate_PAMobileNumber = c.String(),
                        candidate_CAHouseNo = c.String(),
                        candidate_CAMooAndSoi = c.String(),
                        candidate_CAStreet = c.String(),
                        candidate_CAPostalCode = c.String(),
                        candidate_CATelephoneNumber = c.String(),
                        candidate_CAMobileNumber = c.String(),
                        candidate_BankAccountName = c.String(),
                        candidate_BankAccountNumber = c.String(),
                        candidate_SocialSecurityTH = c.String(),
                        candidate_ProvidentFundTH = c.String(),
                        candidate_DeathContribution = c.String(),
                        candidate_EducationStartDate = c.DateTime(),
                        candidate_EducationEndDate = c.DateTime(),
                        candidate_EduInstituteOrLocationOfTraining = c.String(),
                        candidate_EduCountry = c.String(),
                        candidate_EduCurrentGPATranscript = c.String(),
                        candidate_EduCurrentOrLatestDegree = c.String(),
                        candidate_EduMajorStudy = c.String(),
                        candidate_EnglishTestName = c.String(),
                        candidate_EnglishTestScoreOrOxford = c.Decimal(precision: 18, scale: 2),
                        candidate_EnglishTestStatus = c.String(),
                        candidate_EnglishTestDate = c.DateTime(),
                        candidate_TraineeNumber = c.String(),
                        candidate_MilitaryServicesDoc = c.String(),
                        candidate_IBMP = c.String(),
                        candidate_AuditingScore = c.Decimal(precision: 18, scale: 2),
                        candidate_AuditingTestDate = c.DateTime(),
                        candidate_Email = c.String(),
                        candidate_Program = c.String(),
                        candidate_IndustryPrerences1 = c.String(),
                        candidate_IndustryPrerences2 = c.String(),
                        candidate_IndustryPrerences3 = c.String(),
                        candidate_IndustryPrerences4 = c.String(),
                        candidate_IndustryPrerences5 = c.String(),
                        candidate_OfficialNote = c.String(),
                        candidate_InternalNoteForHRTeam = c.String(),
                        candidate_OtherAllowances = c.Decimal(precision: 18, scale: 2),
                        CA_TM_SubDistrict_Id = c.Int(),
                        Gender_Id = c.Int(),
                        MaritalStatusName_Id = c.Int(),
                        PA_TM_SubDistrict_Id = c.Int(),
                        TM_Candidate_Type_Id = c.Int(),
                        TM_SourcingChannel_Id = c.Int(),
                        TM_SubDistrict_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_SubDistrict", t => t.CA_TM_SubDistrict_Id)
                .ForeignKey("dbo.TM_Gender", t => t.Gender_Id)
                .ForeignKey("dbo.TM_MaritalStatus", t => t.MaritalStatusName_Id)
                .ForeignKey("dbo.TM_SubDistrict", t => t.PA_TM_SubDistrict_Id)
                .ForeignKey("dbo.TM_Candidate_Type", t => t.TM_Candidate_Type_Id)
                .ForeignKey("dbo.TM_SourcingChannel", t => t.TM_SourcingChannel_Id)
                .ForeignKey("dbo.TM_SubDistrict", t => t.TM_SubDistrict_Id)
                .Index(t => t.CA_TM_SubDistrict_Id)
                .Index(t => t.Gender_Id)
                .Index(t => t.MaritalStatusName_Id)
                .Index(t => t.PA_TM_SubDistrict_Id)
                .Index(t => t.TM_Candidate_Type_Id)
                .Index(t => t.TM_SourcingChannel_Id)
                .Index(t => t.TM_SubDistrict_Id);
            
            CreateTable(
                "dbo.TM_SubDistrict",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        _sub_district_postcode = c.String(maxLength: 6),
                        subdistrict_name_en = c.String(maxLength: 150),
                        subdistrict_name_th = c.String(maxLength: 150),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                        TM_District_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_District", t => t.TM_District_Id)
                .Index(t => t.TM_District_Id);
            
            CreateTable(
                "dbo.TM_District",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        district_name_en = c.String(maxLength: 150),
                        district_name_th = c.String(maxLength: 150),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                        TM_City_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_City", t => t.TM_City_Id)
                .Index(t => t.TM_City_Id);
            
            CreateTable(
                "dbo.TM_City",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        city_code = c.String(maxLength: 10),
                        city_name_en = c.String(maxLength: 150),
                        city_name_th = c.String(maxLength: 150),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                        TM_Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Country", t => t.TM_Country_Id)
                .Index(t => t.TM_Country_Id);
            
            CreateTable(
                "dbo.TM_Country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        country_name_th = c.String(maxLength: 250),
                        country_name_en = c.String(maxLength: 250),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Gender",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenderId = c.String(maxLength: 50),
                        GenderNameTH = c.String(maxLength: 250),
                        GenderNameEN = c.String(maxLength: 250),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_MaritalStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaritalStatusId = c.String(maxLength: 50),
                        MaritalStatusNameTH = c.String(maxLength: 250),
                        MaritalStatusNameEN = c.String(maxLength: 250),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Candidate_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        candidate_type_name_en = c.String(maxLength: 250),
                        candidate_type_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Education_History",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        grade = c.Decimal(precision: 18, scale: 2),
                        start_date = c.DateTime(),
                        end_date = c.DateTime(),
                        education_history_description = c.String(maxLength: 500),
                        Ref_Cert_ID = c.String(maxLength: 10),
                        Degree = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Candidates_Id = c.Int(),
                        TM_Education_Degree_Id = c.Int(),
                        TM_Universitys_Major_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidates", t => t.TM_Candidates_Id)
                .ForeignKey("dbo.TM_Education_Degree", t => t.TM_Education_Degree_Id)
                .ForeignKey("dbo.TM_Universitys_Major", t => t.TM_Universitys_Major_Id)
                .Index(t => t.TM_Candidates_Id)
                .Index(t => t.TM_Education_Degree_Id)
                .Index(t => t.TM_Universitys_Major_Id);
            
            CreateTable(
                "dbo.TM_Education_Degree",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        degree_name_en = c.String(maxLength: 350),
                        degree_name_th = c.String(maxLength: 350),
                        degree_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Universitys_Major",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        universitys_major_name_en = c.String(maxLength: 350),
                        universitys_major_name_th = c.String(maxLength: 350),
                        universitys_major_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Major_Id = c.Int(),
                        TM_Universitys_Faculty_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Major", t => t.TM_Major_Id)
                .ForeignKey("dbo.TM_Universitys_Faculty", t => t.TM_Universitys_Faculty_Id)
                .Index(t => t.TM_Major_Id)
                .Index(t => t.TM_Universitys_Faculty_Id);
            
            CreateTable(
                "dbo.TM_Major",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        major_name_en = c.String(maxLength: 350),
                        major_name_th = c.String(maxLength: 350),
                        major_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Universitys_Faculty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        universitys_faculty_name_en = c.String(maxLength: 350),
                        universitys_faculty_name_th = c.String(maxLength: 350),
                        universitys_faculty_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Faculty_Id = c.Int(),
                        TM_Universitys_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Faculty", t => t.TM_Faculty_Id)
                .ForeignKey("dbo.TM_Universitys", t => t.TM_Universitys_Id)
                .Index(t => t.TM_Faculty_Id)
                .Index(t => t.TM_Universitys_Id);
            
            CreateTable(
                "dbo.TM_Faculty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        faculty_name_en = c.String(maxLength: 350),
                        faculty_name_th = c.String(maxLength: 350),
                        faculty_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Universitys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        universitys_name_th = c.String(maxLength: 350),
                        universitys_name_en = c.String(maxLength: 350),
                        universitys_short_name_th = c.String(maxLength: 250),
                        universitys_aol_name = c.String(maxLength: 350),
                        universitys_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_SourcingChannel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        sourcingchannel_name_en = c.String(maxLength: 250),
                        sourcingchannel_description = c.String(maxLength: 500),
                        other_status = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_TechnicalTestTransaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Test_Score = c.Decimal(precision: 18, scale: 2),
                        Test_Date = c.DateTime(),
                        active_status = c.String(),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_TechnicalTest_Id = c.Int(),
                        TM_Candidates_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_TechnicalTest", t => t.TM_TechnicalTest_Id)
                .ForeignKey("dbo.TM_Candidates", t => t.TM_Candidates_Id)
                .Index(t => t.TM_TechnicalTest_Id)
                .Index(t => t.TM_Candidates_Id);
            
            CreateTable(
                "dbo.TM_TechnicalTest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Test_name_en = c.String(maxLength: 250),
                        Test_name_th = c.String(maxLength: 250),
                        Test_description = c.String(maxLength: 500),
                        Test_Status = c.String(),
                        active_status = c.String(),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Recruitment_Team",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        User_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PR_Status",
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
                "dbo.TBmst_FYPlan",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Pool = c.String(),
                        LOB = c.String(),
                        FY = c.String(),
                        ResigneeForcast = c.String(),
                        EstimatePreviousFY = c.Int(nullable: false),
                        NewHire = c.Int(nullable: false),
                        Replace = c.Int(nullable: false),
                        epPTR = c.Int(nullable: false),
                        epDIR = c.Int(nullable: false),
                        epAD1 = c.Int(nullable: false),
                        epAD2 = c.Int(nullable: false),
                        epMGR1 = c.Int(nullable: false),
                        epMGR2 = c.Int(nullable: false),
                        epMGR3 = c.Int(nullable: false),
                        epMGR4 = c.Int(nullable: false),
                        epMGR5 = c.Int(nullable: false),
                        epAM1 = c.Int(nullable: false),
                        epAM2 = c.Int(nullable: false),
                        epSR1 = c.Int(nullable: false),
                        epSR2 = c.Int(nullable: false),
                        epAA1 = c.Int(nullable: false),
                        epAA2 = c.Int(nullable: false),
                        epPARA = c.Int(nullable: false),
                        epEA3 = c.Int(nullable: false),
                        epEA2 = c.Int(nullable: false),
                        epEA1 = c.Int(nullable: false),
                        nhPTR = c.Int(nullable: false),
                        nhDIR = c.Int(nullable: false),
                        nhAD1 = c.Int(nullable: false),
                        nhAD2 = c.Int(nullable: false),
                        nhMGR1 = c.Int(nullable: false),
                        nhMGR2 = c.Int(nullable: false),
                        nhMGR3 = c.Int(nullable: false),
                        nhMGR4 = c.Int(nullable: false),
                        nhMGR5 = c.Int(nullable: false),
                        nhAM1 = c.Int(nullable: false),
                        nhAM2 = c.Int(nullable: false),
                        nhSR1 = c.Int(nullable: false),
                        nhSR2 = c.Int(nullable: false),
                        nhAA1 = c.Int(nullable: false),
                        nhAA2 = c.Int(nullable: false),
                        nhPARA = c.Int(nullable: false),
                        nhEA1 = c.Int(nullable: false),
                        nhEA2 = c.Int(nullable: false),
                        nhEA3 = c.Int(nullable: false),
                        rpPTR = c.Int(nullable: false),
                        rpDIR = c.Int(nullable: false),
                        rpAD1 = c.Int(nullable: false),
                        rpAD2 = c.Int(nullable: false),
                        rpMGR1 = c.Int(nullable: false),
                        rpMGR2 = c.Int(nullable: false),
                        rpMGR3 = c.Int(nullable: false),
                        rpMGR4 = c.Int(nullable: false),
                        rpMGR5 = c.Int(nullable: false),
                        rpAM1 = c.Int(nullable: false),
                        rpAM2 = c.Int(nullable: false),
                        rpSR1 = c.Int(nullable: false),
                        rpSR2 = c.Int(nullable: false),
                        rpAA1 = c.Int(nullable: false),
                        rpAA2 = c.Int(nullable: false),
                        rpPARA = c.Int(nullable: false),
                        rpEA1 = c.Int(nullable: false),
                        rpEA2 = c.Int(nullable: false),
                        rpEA3 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupListPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        action_type_permiss = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        GroupPermission_Id = c.Int(),
                        MenuAction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupPermissions", t => t.GroupPermission_Id)
                .ForeignKey("dbo.MenuActions", t => t.MenuAction_Id)
                .Index(t => t.GroupPermission_Id)
                .Index(t => t.MenuAction_Id);
            
            CreateTable(
                "dbo.GroupPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        group_name_th = c.String(maxLength: 250),
                        group_name_en = c.String(maxLength: 250),
                        group_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Controller = c.String(maxLength: 500),
                        Action = c.String(maxLength: 500),
                        ACTIVE_FLAG = c.String(maxLength: 10),
                        CREATED_DT = c.DateTime(),
                        CREATED_USER = c.String(maxLength: 50),
                        UPDATE_DT = c.DateTime(),
                        UPDATE_USER = c.String(maxLength: 50),
                        Action_Type = c.Int(nullable: false),
                        Menu_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .Index(t => t.Menu_Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MENU_ID = c.String(maxLength: 10),
                        MENU_PARENT = c.String(maxLength: 10),
                        MENU_LEVEL = c.Int(nullable: false),
                        MENU_NAME_TH = c.String(maxLength: 500),
                        MENU_NAME_EN = c.String(maxLength: 500),
                        MENU_SEQ = c.Int(nullable: false),
                        Controller = c.String(maxLength: 500),
                        Action = c.String(maxLength: 500),
                        LINK = c.String(maxLength: 250),
                        CREATED_DT = c.DateTime(),
                        CREATED_USER = c.String(maxLength: 50),
                        UPDATE_DT = c.DateTime(),
                        UPDATE_USER = c.String(maxLength: 50),
                        ACTIVE_FLAG = c.String(maxLength: 10),
                        MENU_SUB = c.String(maxLength: 10),
                        MENU_ICON = c.String(maxLength: 50),
                        MENU_DUMMY = c.String(maxLength: 50),
                        active_menu = c.String(maxLength: 50),
                        MENU_Permission = c.String(maxLength: 10),
                        menu_type = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TBmst_PreviousFY",
                c => new
                    {
                        EmployeeNo = c.String(nullable: false, maxLength: 128),
                        EmployeeName = c.String(nullable: false),
                        EmployeeSurname = c.String(nullable: false),
                        Company = c.String(nullable: false),
                        Pool = c.String(nullable: false),
                        UnitGroup = c.String(nullable: false),
                        Rank = c.String(nullable: false),
                        RankCode = c.String(nullable: false),
                        FullRank = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeNo);
            
            CreateTable(
                "dbo.PTR_Evaluation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        active_status = c.String(maxLength: 10),
                        comments = c.String(maxLength: 1500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        Final_Annual_Rating_Id = c.Int(),
                        PTR_Evaluation_Year_Id = c.Int(nullable: false),
                        Self_Annual_Rating_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Annual_Rating", t => t.Final_Annual_Rating_Id)
                .ForeignKey("dbo.PTR_Evaluation_Year", t => t.PTR_Evaluation_Year_Id, cascadeDelete: true)
                .ForeignKey("dbo.TM_Annual_Rating", t => t.Self_Annual_Rating_Id)
                .Index(t => t.Final_Annual_Rating_Id)
                .Index(t => t.PTR_Evaluation_Year_Id)
                .Index(t => t.Self_Annual_Rating_Id);
            
            CreateTable(
                "dbo.TM_Annual_Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        rating_code = c.String(maxLength: 50),
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
                "dbo.PTR_Evaluation_Answer",
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
                        PTR_Evaluation_Id = c.Int(),
                        TM_PTR_Eva_Questions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .ForeignKey("dbo.TM_PTR_Eva_Questions", t => t.TM_PTR_Eva_Questions_Id)
                .Index(t => t.PTR_Evaluation_Id)
                .Index(t => t.TM_PTR_Eva_Questions_Id);
            
            CreateTable(
                "dbo.TM_PTR_Eva_Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        qgroup = c.String(maxLength: 300),
                        header = c.String(maxLength: 300),
                        question = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        questions_type = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Partner_Evaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Partner_Evaluation", t => t.TM_Partner_Evaluation_Id)
                .Index(t => t.TM_Partner_Evaluation_Id);
            
            CreateTable(
                "dbo.TM_Partner_Evaluation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        header = c.String(maxLength: 500),
                        instruction = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        action_date = c.DateTime(),
                        description = c.String(maxLength: 500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        score_remark = c.String(maxLength: 1000),
                        score_rate = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PTR_Eva_KPIs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        min_target = c.Int(),
                        max_target = c.Int(nullable: false),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_KPIs_Base_Id = c.Int(),
                        TM_Partner_Evaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_KPIs_Base", t => t.TM_KPIs_Base_Id)
                .ForeignKey("dbo.TM_Partner_Evaluation", t => t.TM_Partner_Evaluation_Id)
                .Index(t => t.TM_KPIs_Base_Id)
                .Index(t => t.TM_Partner_Evaluation_Id);
            
            CreateTable(
                "dbo.TM_KPIs_Base",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        kpi_name_en = c.String(maxLength: 250),
                        kpi_description = c.String(maxLength: 500),
                        kpi_xlxs = c.String(maxLength: 250),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PTR_Eva_Score",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        qgroup = c.String(maxLength: 300),
                        question = c.String(maxLength: 1000),
                        score = c.Decimal(nullable: false, precision: 18, scale: 2),
                        sub_total = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Partner_Evaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Partner_Evaluation", t => t.TM_Partner_Evaluation_Id)
                .Index(t => t.TM_Partner_Evaluation_Id);
            
            CreateTable(
                "dbo.PTR_Evaluation_File",
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
                        PTR_Evaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .Index(t => t.PTR_Evaluation_Id);
            
            CreateTable(
                "dbo.PTR_Evaluation_KPIs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        how_to = c.String(),
                        final_remark = c.String(),
                        target = c.String(maxLength: 1000),
                        actual = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        PTR_Evaluation_Id = c.Int(),
                        TM_PTR_Eva_Questions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .ForeignKey("dbo.TM_PTR_Eva_Questions", t => t.TM_PTR_Eva_Questions_Id)
                .Index(t => t.PTR_Evaluation_Id)
                .Index(t => t.TM_PTR_Eva_Questions_Id);
            
            CreateTable(
                "dbo.PTR_Evaluation_Year",
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TBmst_Status",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StatusDesc = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TBtran_PRForm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RefNo = c.String(nullable: false),
                        CompanyCode = c.String(nullable: false),
                        Division = c.String(nullable: false),
                        Remark = c.String(),
                        JobDescription = c.String(),
                        EducationDesc = c.String(),
                        Language = c.String(),
                        RequestBy = c.String(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        BUApproveBy = c.String(),
                        BUApproveDate = c.DateTime(),
                        HeadApproveBy = c.String(),
                        HeadApproveDate = c.DateTime(),
                        CeoApproveBy = c.String(),
                        CeoApproveDate = c.DateTime(),
                        rejectReason = c.String(),
                        RequestType_Id = c.Long(nullable: false),
                        Status_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TBmst_RequestType", t => t.RequestType_Id, cascadeDelete: true)
                .ForeignKey("dbo.TBmst_Status", t => t.Status_Id, cascadeDelete: true)
                .Index(t => t.RequestType_Id)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.TBmst_AttachFile",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        PRForm_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TBtran_PRForm", t => t.PRForm_Id)
                .Index(t => t.PRForm_Id);
            
            CreateTable(
                "dbo.TBtran_Candidate",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CandidateName = c.String(nullable: false),
                        rank = c.String(nullable: false),
                        fullRank = c.String(),
                        ContactNo = c.String(),
                        AcceptDate = c.DateTime(),
                        OfferingDate = c.DateTime(),
                        DateByBU = c.DateTime(),
                        StartDate = c.DateTime(),
                        RejectedDate = c.DateTime(),
                        Remark = c.String(),
                        HROwner = c.String(),
                        HiringProcess_Id = c.Long(),
                        InterviewStatus_Id = c.Long(),
                        PRForm_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TBmst_HiringProcess", t => t.HiringProcess_Id)
                .ForeignKey("dbo.TBmst_InterviewStatus", t => t.InterviewStatus_Id)
                .ForeignKey("dbo.TBtran_PRForm", t => t.PRForm_Id, cascadeDelete: true)
                .Index(t => t.HiringProcess_Id)
                .Index(t => t.InterviewStatus_Id)
                .Index(t => t.PRForm_Id);
            
            CreateTable(
                "dbo.TBmst_HiringProcess",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HiringProcessDesc = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TBmst_InterviewStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InterviewDesc = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TBtran_Position",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PositionTitle = c.String(),
                        Rank = c.String(),
                        TargetPeriod = c.DateTime(nullable: false),
                        TargetPeriodTo = c.DateTime(nullable: false),
                        Headcount = c.Int(nullable: false),
                        Specify = c.String(),
                        Headcount1 = c.Int(nullable: false),
                        Headcount2 = c.Int(nullable: false),
                        Headcount3 = c.Int(nullable: false),
                        Headcount4 = c.Int(nullable: false),
                        Headcount5 = c.Int(nullable: false),
                        Headcount6 = c.Int(nullable: false),
                        Headcount7 = c.Int(nullable: false),
                        EmpType_Id = c.Long(nullable: false),
                        PRForm_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TBmst_EmployeeType", t => t.EmpType_Id, cascadeDelete: true)
                .ForeignKey("dbo.TBtran_PRForm", t => t.PRForm_Id, cascadeDelete: true)
                .Index(t => t.EmpType_Id)
                .Index(t => t.PRForm_Id);
            
            CreateTable(
                "dbo.TBmst_EmployeeType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmployeeTypeDesc = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TBmst_RequestType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestTypeDesc = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Candidate_MassTIF",
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
                        TM_Mass_Question_Type_Id = c.Int(),
                        TM_MassTIF_Status_Id = c.Int(),
                        TM_PR_Candidate_Mapping_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Pool_Rank", t => t.Recommended_Rank_Id)
                .ForeignKey("dbo.TM_Mass_Question_Type", t => t.TM_Mass_Question_Type_Id)
                .ForeignKey("dbo.TM_MassTIF_Status", t => t.TM_MassTIF_Status_Id)
                .ForeignKey("dbo.TM_PR_Candidate_Mapping", t => t.TM_PR_Candidate_Mapping_Id, cascadeDelete: true)
                .Index(t => t.Recommended_Rank_Id)
                .Index(t => t.TM_Mass_Question_Type_Id)
                .Index(t => t.TM_MassTIF_Status_Id)
                .Index(t => t.TM_PR_Candidate_Mapping_Id);
            
            CreateTable(
                "dbo.TM_Candidate_MassTIF_Approv",
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
                        TM_Candidate_MassTIF_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_MassTIF", t => t.TM_Candidate_MassTIF_Id)
                .Index(t => t.TM_Candidate_MassTIF_Id);
            
            CreateTable(
                "dbo.TM_Candidate_MassTIF_Audit_Qst",
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
                        TM_Candidate_MassTIF_Id = c.Int(),
                        TM_Mass_Auditing_Question_Id = c.Int(),
                        TM_Mass_Scoring_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_MassTIF", t => t.TM_Candidate_MassTIF_Id)
                .ForeignKey("dbo.TM_Mass_Auditing_Question", t => t.TM_Mass_Auditing_Question_Id)
                .ForeignKey("dbo.TM_Mass_Scoring", t => t.TM_Mass_Scoring_Id)
                .Index(t => t.TM_Candidate_MassTIF_Id)
                .Index(t => t.TM_Mass_Auditing_Question_Id)
                .Index(t => t.TM_Mass_Scoring_Id);
            
            CreateTable(
                "dbo.TM_Mass_Auditing_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        header = c.String(maxLength: 1000),
                        question = c.String(maxLength: 1000),
                        answer_guideline = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Mass_Question_Type_Id = c.Int(),
                        TM_Mass_TIF_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Mass_Question_Type", t => t.TM_Mass_Question_Type_Id)
                .ForeignKey("dbo.TM_Mass_TIF_Form", t => t.TM_Mass_TIF_Form_Id)
                .Index(t => t.TM_Mass_Question_Type_Id)
                .Index(t => t.TM_Mass_TIF_Form_Id);
            
            CreateTable(
                "dbo.TM_Mass_Question_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        question_type_name_th = c.String(maxLength: 250),
                        question_type_name_en = c.String(maxLength: 250),
                        question_type_description = c.String(maxLength: 500),
                        point = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Mass_TIF_Form",
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
                "dbo.TM_MassTIF_Form_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        header = c.String(maxLength: 1000),
                        question = c.String(maxLength: 1000),
                        a_answer = c.String(),
                        b_answer = c.String(),
                        c_answer = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Mass_TIF_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Mass_TIF_Form", t => t.TM_Mass_TIF_Form_Id)
                .Index(t => t.TM_Mass_TIF_Form_Id);
            
            CreateTable(
                "dbo.TM_Mass_Scoring",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        scoring_code = c.String(maxLength: 50),
                        scoring_name_th = c.String(maxLength: 250),
                        scoring_name_en = c.String(maxLength: 250),
                        scoring_description = c.String(maxLength: 500),
                        point = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Candidate_MassTIF_Core",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        evidence = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Candidate_MassTIF_Id = c.Int(),
                        TM_Mass_Scoring_Id = c.Int(),
                        TM_MassTIF_Form_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_MassTIF", t => t.TM_Candidate_MassTIF_Id)
                .ForeignKey("dbo.TM_Mass_Scoring", t => t.TM_Mass_Scoring_Id)
                .ForeignKey("dbo.TM_MassTIF_Form_Question", t => t.TM_MassTIF_Form_Question_Id)
                .Index(t => t.TM_Candidate_MassTIF_Id)
                .Index(t => t.TM_Mass_Scoring_Id)
                .Index(t => t.TM_MassTIF_Form_Question_Id);
            
            CreateTable(
                "dbo.TM_MassTIF_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        masstif_status_name_th = c.String(maxLength: 250),
                        masstif_status_name_en = c.String(maxLength: 250),
                        masstif_short_name_en = c.String(maxLength: 250),
                        masstif_status_description = c.String(maxLength: 500),
                        type_status = c.String(maxLength: 10),
                        status_id = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Candidate_TIF",
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
                        TM_PR_Candidate_Mapping_Id = c.Int(nullable: false),
                        TM_TIF_Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Pool_Rank", t => t.Recommended_Rank_Id)
                .ForeignKey("dbo.TM_PR_Candidate_Mapping", t => t.TM_PR_Candidate_Mapping_Id, cascadeDelete: true)
                .ForeignKey("dbo.TM_TIF_Status", t => t.TM_TIF_Status_Id)
                .Index(t => t.Recommended_Rank_Id)
                .Index(t => t.TM_PR_Candidate_Mapping_Id)
                .Index(t => t.TM_TIF_Status_Id);
            
            CreateTable(
                "dbo.TM_Candidate_TIF_Answer",
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
                        TM_Candidate_TIF_Id = c.Int(),
                        TM_TIF_Form_Question_Id = c.Int(),
                        TM_TIF_Rating_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_TIF", t => t.TM_Candidate_TIF_Id)
                .ForeignKey("dbo.TM_TIF_Form_Question", t => t.TM_TIF_Form_Question_Id)
                .ForeignKey("dbo.TM_TIF_Rating", t => t.TM_TIF_Rating_Id)
                .Index(t => t.TM_Candidate_TIF_Id)
                .Index(t => t.TM_TIF_Form_Question_Id)
                .Index(t => t.TM_TIF_Rating_Id);
            
            CreateTable(
                "dbo.TM_TIF_Form_Question",
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
                        TM_TIF_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_TIF_Form", t => t.TM_TIF_Form_Id)
                .Index(t => t.TM_TIF_Form_Id);
            
            CreateTable(
                "dbo.TM_TIF_Form",
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
                "dbo.TM_TIF_Rating",
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
                "dbo.TM_Candidate_TIF_Approv",
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
                        TM_Candidate_TIF_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidate_TIF", t => t.TM_Candidate_TIF_Id)
                .Index(t => t.TM_Candidate_TIF_Id);
            
            CreateTable(
                "dbo.TM_TIF_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        tif_status_name_th = c.String(maxLength: 250),
                        tif_status_name_en = c.String(maxLength: 250),
                        tif_short_name_en = c.String(maxLength: 250),
                        tif_status_description = c.String(maxLength: 500),
                        status_id = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Employment_Rank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Employment_Type_Id = c.Int(),
                        TM_Rank_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Employment_Type", t => t.TM_Employment_Type_Id)
                .ForeignKey("dbo.TM_Rank", t => t.TM_Rank_Id)
                .Index(t => t.TM_Employment_Type_Id)
                .Index(t => t.TM_Rank_Id);
            
            CreateTable(
                "dbo.TM_Eva_Rating",
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
                "dbo.TM_Evaluation_Form",
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
                "dbo.TM_Evaluation_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        qgroup = c.String(maxLength: 300),
                        header = c.String(maxLength: 300),
                        question = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Evaluation_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Evaluation_Form", t => t.TM_Evaluation_Form_Id)
                .Index(t => t.TM_Evaluation_Form_Id);
            
            CreateTable(
                "dbo.TM_Flow_Approve",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        flow_name_th = c.String(maxLength: 250),
                        flow_name_en = c.String(maxLength: 250),
                        flow_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Step_Approve",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        step_name_en = c.String(maxLength: 250),
                        step_short_name_en = c.String(maxLength: 250),
                        step_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        step = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Trainee_Eva",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        hr_acknowledge = c.String(maxLength: 10),
                        acknowledge_date = c.DateTime(),
                        acknowledge_user = c.String(maxLength: 50),
                        submit_status = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        trainee_comments = c.String(maxLength: 1500),
                        incharge_comments = c.String(maxLength: 1500),
                        hiring_status = c.String(maxLength: 10),
                        req_incharge_Approve_user = c.String(maxLength: 50),
                        incharge_Approve_date = c.DateTime(),
                        incharge_Approve_user = c.String(maxLength: 50),
                        incharge_Approve_status = c.String(maxLength: 10),
                        incharge_Approve_remark = c.String(maxLength: 500),
                        req_mgr_Approve_user = c.String(maxLength: 50),
                        mgr_Approve_date = c.DateTime(),
                        mgr_Approve_user = c.String(maxLength: 50),
                        mgr_Approve_status = c.String(maxLength: 10),
                        mgr_Approve_remark = c.String(maxLength: 500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_PR_Candidate_Mapping_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_PR_Candidate_Mapping", t => t.TM_PR_Candidate_Mapping_Id)
                .Index(t => t.TM_PR_Candidate_Mapping_Id);
            
            CreateTable(
                "dbo.TM_Trainee_Eva_Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        inchage_comment = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        inchage_rating_Id = c.Int(),
                        TM_Evaluation_Question_Id = c.Int(),
                        TM_Trainee_Eva_Id = c.Int(),
                        trainee_rating_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Eva_Rating", t => t.inchage_rating_Id)
                .ForeignKey("dbo.TM_Evaluation_Question", t => t.TM_Evaluation_Question_Id)
                .ForeignKey("dbo.TM_Trainee_Eva", t => t.TM_Trainee_Eva_Id)
                .ForeignKey("dbo.TM_Eva_Rating", t => t.trainee_rating_Id)
                .Index(t => t.inchage_rating_Id)
                .Index(t => t.TM_Evaluation_Question_Id)
                .Index(t => t.TM_Trainee_Eva_Id)
                .Index(t => t.trainee_rating_Id);
            
            CreateTable(
                "dbo.TraineeMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MENU_ID = c.String(maxLength: 10),
                        MENU_PARENT = c.String(maxLength: 10),
                        MENU_LEVEL = c.Int(nullable: false),
                        MENU_NAME_TH = c.String(maxLength: 500),
                        MENU_NAME_EN = c.String(maxLength: 500),
                        MENU_SEQ = c.Int(nullable: false),
                        Controller = c.String(maxLength: 500),
                        Action = c.String(maxLength: 500),
                        LINK = c.String(maxLength: 250),
                        CREATED_DT = c.DateTime(),
                        CREATED_USER = c.String(maxLength: 50),
                        UPDATE_DT = c.DateTime(),
                        UPDATE_USER = c.String(maxLength: 50),
                        ACTIVE_FLAG = c.String(maxLength: 10),
                        MENU_SUB = c.String(maxLength: 10),
                        MENU_ICON = c.String(maxLength: 50),
                        MENU_DUMMY = c.String(maxLength: 50),
                        active_menu = c.String(maxLength: 50),
                        MENU_Permission = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TraineeMenuActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Controller = c.String(maxLength: 500),
                        Action = c.String(maxLength: 500),
                        ACTIVE_FLAG = c.String(maxLength: 10),
                        CREATED_DT = c.DateTime(),
                        CREATED_USER = c.String(maxLength: 50),
                        UPDATE_DT = c.DateTime(),
                        UPDATE_USER = c.String(maxLength: 50),
                        Action_Type = c.Int(nullable: false),
                        TraineeMenu_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TraineeMenus", t => t.TraineeMenu_Id)
                .Index(t => t.TraineeMenu_Id);
            
            CreateTable(
                "dbo.UserListPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        MenuAction_Id = c.Int(),
                        UserPermission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuActions", t => t.MenuAction_Id)
                .ForeignKey("dbo.UserPermissions", t => t.UserPermission_Id)
                .Index(t => t.MenuAction_Id)
                .Index(t => t.UserPermission_Id);
            
            CreateTable(
                "dbo.UserPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        User_description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        GroupPermission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupPermissions", t => t.GroupPermission_Id)
                .Index(t => t.GroupPermission_Id);
            
            CreateTable(
                "dbo.UserUnitGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        unit_code = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Divisions_Id = c.Int(),
                        UserPermission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Divisions", t => t.TM_Divisions_Id)
                .ForeignKey("dbo.UserPermissions", t => t.UserPermission_Id)
                .Index(t => t.TM_Divisions_Id)
                .Index(t => t.UserPermission_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserUnitGroups", "UserPermission_Id", "dbo.UserPermissions");
            DropForeignKey("dbo.UserUnitGroups", "TM_Divisions_Id", "dbo.TM_Divisions");
            DropForeignKey("dbo.UserListPermissions", "UserPermission_Id", "dbo.UserPermissions");
            DropForeignKey("dbo.UserPermissions", "GroupPermission_Id", "dbo.GroupPermissions");
            DropForeignKey("dbo.UserListPermissions", "MenuAction_Id", "dbo.MenuActions");
            DropForeignKey("dbo.TraineeMenuActions", "TraineeMenu_Id", "dbo.TraineeMenus");
            DropForeignKey("dbo.TM_Trainee_Eva_Answer", "trainee_rating_Id", "dbo.TM_Eva_Rating");
            DropForeignKey("dbo.TM_Trainee_Eva_Answer", "TM_Trainee_Eva_Id", "dbo.TM_Trainee_Eva");
            DropForeignKey("dbo.TM_Trainee_Eva_Answer", "TM_Evaluation_Question_Id", "dbo.TM_Evaluation_Question");
            DropForeignKey("dbo.TM_Trainee_Eva_Answer", "inchage_rating_Id", "dbo.TM_Eva_Rating");
            DropForeignKey("dbo.TM_Trainee_Eva", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.TM_Evaluation_Question", "TM_Evaluation_Form_Id", "dbo.TM_Evaluation_Form");
            DropForeignKey("dbo.TM_Employment_Rank", "TM_Rank_Id", "dbo.TM_Rank");
            DropForeignKey("dbo.TM_Employment_Rank", "TM_Employment_Type_Id", "dbo.TM_Employment_Type");
            DropForeignKey("dbo.TM_Candidate_TIF", "TM_TIF_Status_Id", "dbo.TM_TIF_Status");
            DropForeignKey("dbo.TM_Candidate_TIF", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.TM_Candidate_TIF_Approv", "TM_Candidate_TIF_Id", "dbo.TM_Candidate_TIF");
            DropForeignKey("dbo.TM_Candidate_TIF_Answer", "TM_TIF_Rating_Id", "dbo.TM_TIF_Rating");
            DropForeignKey("dbo.TM_Candidate_TIF_Answer", "TM_TIF_Form_Question_Id", "dbo.TM_TIF_Form_Question");
            DropForeignKey("dbo.TM_TIF_Form_Question", "TM_TIF_Form_Id", "dbo.TM_TIF_Form");
            DropForeignKey("dbo.TM_Candidate_TIF_Answer", "TM_Candidate_TIF_Id", "dbo.TM_Candidate_TIF");
            DropForeignKey("dbo.TM_Candidate_TIF", "Recommended_Rank_Id", "dbo.TM_Pool_Rank");
            DropForeignKey("dbo.TM_Candidate_MassTIF", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.TM_Candidate_MassTIF", "TM_MassTIF_Status_Id", "dbo.TM_MassTIF_Status");
            DropForeignKey("dbo.TM_Candidate_MassTIF", "TM_Mass_Question_Type_Id", "dbo.TM_Mass_Question_Type");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Core", "TM_MassTIF_Form_Question_Id", "dbo.TM_MassTIF_Form_Question");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Core", "TM_Mass_Scoring_Id", "dbo.TM_Mass_Scoring");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Core", "TM_Candidate_MassTIF_Id", "dbo.TM_Candidate_MassTIF");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Audit_Qst", "TM_Mass_Scoring_Id", "dbo.TM_Mass_Scoring");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Audit_Qst", "TM_Mass_Auditing_Question_Id", "dbo.TM_Mass_Auditing_Question");
            DropForeignKey("dbo.TM_MassTIF_Form_Question", "TM_Mass_TIF_Form_Id", "dbo.TM_Mass_TIF_Form");
            DropForeignKey("dbo.TM_Mass_Auditing_Question", "TM_Mass_TIF_Form_Id", "dbo.TM_Mass_TIF_Form");
            DropForeignKey("dbo.TM_Mass_Auditing_Question", "TM_Mass_Question_Type_Id", "dbo.TM_Mass_Question_Type");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Audit_Qst", "TM_Candidate_MassTIF_Id", "dbo.TM_Candidate_MassTIF");
            DropForeignKey("dbo.TM_Candidate_MassTIF_Approv", "TM_Candidate_MassTIF_Id", "dbo.TM_Candidate_MassTIF");
            DropForeignKey("dbo.TM_Candidate_MassTIF", "Recommended_Rank_Id", "dbo.TM_Pool_Rank");
            DropForeignKey("dbo.TBtran_PRForm", "Status_Id", "dbo.TBmst_Status");
            DropForeignKey("dbo.TBtran_PRForm", "RequestType_Id", "dbo.TBmst_RequestType");
            DropForeignKey("dbo.TBtran_Position", "PRForm_Id", "dbo.TBtran_PRForm");
            DropForeignKey("dbo.TBtran_Position", "EmpType_Id", "dbo.TBmst_EmployeeType");
            DropForeignKey("dbo.TBtran_Candidate", "PRForm_Id", "dbo.TBtran_PRForm");
            DropForeignKey("dbo.TBtran_Candidate", "InterviewStatus_Id", "dbo.TBmst_InterviewStatus");
            DropForeignKey("dbo.TBtran_Candidate", "HiringProcess_Id", "dbo.TBmst_HiringProcess");
            DropForeignKey("dbo.TBmst_AttachFile", "PRForm_Id", "dbo.TBtran_PRForm");
            DropForeignKey("dbo.PTR_Evaluation", "Self_Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropForeignKey("dbo.PTR_Evaluation", "PTR_Evaluation_Year_Id", "dbo.PTR_Evaluation_Year");
            DropForeignKey("dbo.PTR_Evaluation_KPIs", "TM_PTR_Eva_Questions_Id", "dbo.TM_PTR_Eva_Questions");
            DropForeignKey("dbo.PTR_Evaluation_KPIs", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropForeignKey("dbo.PTR_Evaluation_File", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropForeignKey("dbo.PTR_Evaluation_Answer", "TM_PTR_Eva_Questions_Id", "dbo.TM_PTR_Eva_Questions");
            DropForeignKey("dbo.TM_PTR_Eva_Score", "TM_Partner_Evaluation_Id", "dbo.TM_Partner_Evaluation");
            DropForeignKey("dbo.TM_PTR_Eva_Questions", "TM_Partner_Evaluation_Id", "dbo.TM_Partner_Evaluation");
            DropForeignKey("dbo.TM_PTR_Eva_KPIs", "TM_Partner_Evaluation_Id", "dbo.TM_Partner_Evaluation");
            DropForeignKey("dbo.TM_PTR_Eva_KPIs", "TM_KPIs_Base_Id", "dbo.TM_KPIs_Base");
            DropForeignKey("dbo.PTR_Evaluation_Answer", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropForeignKey("dbo.PTR_Evaluation", "Final_Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropForeignKey("dbo.GroupListPermissions", "MenuAction_Id", "dbo.MenuActions");
            DropForeignKey("dbo.MenuActions", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.GroupListPermissions", "GroupPermission_Id", "dbo.GroupPermissions");
            DropForeignKey("dbo.E_Mail_History", "PersonnelRequest_Id", "dbo.PersonnelRequests");
            DropForeignKey("dbo.PersonnelRequests", "TM_SubGroup_Id", "dbo.TM_SubGroup");
            DropForeignKey("dbo.PersonnelRequests", "TM_PR_Status_Id", "dbo.TM_PR_Status");
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_Recruitment_Team_Id", "dbo.TM_Recruitment_Team");
            DropForeignKey("dbo.TM_TechnicalTestTransaction", "TM_Candidates_Id", "dbo.TM_Candidates");
            DropForeignKey("dbo.TM_TechnicalTestTransaction", "TM_TechnicalTest_Id", "dbo.TM_TechnicalTest");
            DropForeignKey("dbo.TM_Candidates", "TM_SubDistrict_Id", "dbo.TM_SubDistrict");
            DropForeignKey("dbo.TM_Candidates", "TM_SourcingChannel_Id", "dbo.TM_SourcingChannel");
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_Candidates_Id", "dbo.TM_Candidates");
            DropForeignKey("dbo.TM_Education_History", "TM_Universitys_Major_Id", "dbo.TM_Universitys_Major");
            DropForeignKey("dbo.TM_Universitys_Major", "TM_Universitys_Faculty_Id", "dbo.TM_Universitys_Faculty");
            DropForeignKey("dbo.TM_Universitys_Faculty", "TM_Universitys_Id", "dbo.TM_Universitys");
            DropForeignKey("dbo.TM_Universitys_Faculty", "TM_Faculty_Id", "dbo.TM_Faculty");
            DropForeignKey("dbo.TM_Universitys_Major", "TM_Major_Id", "dbo.TM_Major");
            DropForeignKey("dbo.TM_Education_History", "TM_Education_Degree_Id", "dbo.TM_Education_Degree");
            DropForeignKey("dbo.TM_Education_History", "TM_Candidates_Id", "dbo.TM_Candidates");
            DropForeignKey("dbo.TM_Candidates", "TM_Candidate_Type_Id", "dbo.TM_Candidate_Type");
            DropForeignKey("dbo.TM_Candidates", "PA_TM_SubDistrict_Id", "dbo.TM_SubDistrict");
            DropForeignKey("dbo.TM_Candidates", "MaritalStatusName_Id", "dbo.TM_MaritalStatus");
            DropForeignKey("dbo.TM_Candidates", "Gender_Id", "dbo.TM_Gender");
            DropForeignKey("dbo.TM_Candidates", "CA_TM_SubDistrict_Id", "dbo.TM_SubDistrict");
            DropForeignKey("dbo.TM_SubDistrict", "TM_District_Id", "dbo.TM_District");
            DropForeignKey("dbo.TM_District", "TM_City_Id", "dbo.TM_City");
            DropForeignKey("dbo.TM_City", "TM_Country_Id", "dbo.TM_Country");
            DropForeignKey("dbo.TM_Candidate_Status_Cycle", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.TM_Candidate_Status_Cycle", "TM_Candidate_Status_Id", "dbo.TM_Candidate_Status");
            DropForeignKey("dbo.TM_Candidate_Status_Next", "TM_Candidate_Status_Id", "dbo.TM_Candidate_Status");
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_Candidate_Rank_Id", "dbo.TM_Candidate_Rank");
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "PersonnelRequest_Id", "dbo.PersonnelRequests");
            DropForeignKey("dbo.PersonnelRequests", "TM_Position_Id", "dbo.TM_Position");
            DropForeignKey("dbo.PersonnelRequests", "TM_Pool_Rank_Id", "dbo.TM_Pool_Rank");
            DropForeignKey("dbo.PersonnelRequests", "TM_Employment_Request_Id", "dbo.TM_Employment_Request");
            DropForeignKey("dbo.TM_Employment_Request", "TM_Request_Type_Id", "dbo.TM_Request_Type");
            DropForeignKey("dbo.TM_Employment_Request", "TM_Employment_Type_Id", "dbo.TM_Employment_Type");
            DropForeignKey("dbo.PersonnelRequests", "TM_Divisions_Id", "dbo.TM_Divisions");
            DropForeignKey("dbo.TM_UnitGroup_Approve_Permit", "TM_Divisions_Id", "dbo.TM_Divisions");
            DropForeignKey("dbo.TM_SubGroup", "TM_Divisions_Id", "dbo.TM_Divisions");
            DropForeignKey("dbo.TM_Position", "TM_Divisions_Id", "dbo.TM_Divisions");
            DropForeignKey("dbo.TM_Pool_Rank", "TM_Rank_Id", "dbo.TM_Rank");
            DropForeignKey("dbo.TM_Pool_Rank", "TM_Pool_Id", "dbo.TM_Pool");
            DropForeignKey("dbo.TM_Pool_Approve_Permit", "TM_Pool_Id", "dbo.TM_Pool");
            DropForeignKey("dbo.TM_Divisions", "TM_Pool_Id", "dbo.TM_Pool");
            DropForeignKey("dbo.TM_Pool", "TM_Company_Id", "dbo.TM_Company");
            DropForeignKey("dbo.TM_Company_Approve_Permit", "TM_Company_Id", "dbo.TM_Company");
            DropForeignKey("dbo.E_Mail_History", "MailContent_Id", "dbo.MailContents");
            DropForeignKey("dbo.TM_MailContent_Param", "MailContent_Id", "dbo.MailContents");
            DropForeignKey("dbo.TM_MailContent_Cc_bymail", "MailContent_Id", "dbo.MailContents");
            DropForeignKey("dbo.TM_MailContent_Cc", "MailContent_Id", "dbo.MailContents");
            DropForeignKey("dbo.TBmst_SubTeamMember", "SubTeam_Id", "dbo.TBmst_SubTeam");
            DropForeignKey("dbo.TBmst_SubTeam", "Division_Id", "dbo.TBmst_Division");
            DropIndex("dbo.UserUnitGroups", new[] { "UserPermission_Id" });
            DropIndex("dbo.UserUnitGroups", new[] { "TM_Divisions_Id" });
            DropIndex("dbo.UserPermissions", new[] { "GroupPermission_Id" });
            DropIndex("dbo.UserListPermissions", new[] { "UserPermission_Id" });
            DropIndex("dbo.UserListPermissions", new[] { "MenuAction_Id" });
            DropIndex("dbo.TraineeMenuActions", new[] { "TraineeMenu_Id" });
            DropIndex("dbo.TM_Trainee_Eva_Answer", new[] { "trainee_rating_Id" });
            DropIndex("dbo.TM_Trainee_Eva_Answer", new[] { "TM_Trainee_Eva_Id" });
            DropIndex("dbo.TM_Trainee_Eva_Answer", new[] { "TM_Evaluation_Question_Id" });
            DropIndex("dbo.TM_Trainee_Eva_Answer", new[] { "inchage_rating_Id" });
            DropIndex("dbo.TM_Trainee_Eva", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TM_Evaluation_Question", new[] { "TM_Evaluation_Form_Id" });
            DropIndex("dbo.TM_Employment_Rank", new[] { "TM_Rank_Id" });
            DropIndex("dbo.TM_Employment_Rank", new[] { "TM_Employment_Type_Id" });
            DropIndex("dbo.TM_Candidate_TIF_Approv", new[] { "TM_Candidate_TIF_Id" });
            DropIndex("dbo.TM_TIF_Form_Question", new[] { "TM_TIF_Form_Id" });
            DropIndex("dbo.TM_Candidate_TIF_Answer", new[] { "TM_TIF_Rating_Id" });
            DropIndex("dbo.TM_Candidate_TIF_Answer", new[] { "TM_TIF_Form_Question_Id" });
            DropIndex("dbo.TM_Candidate_TIF_Answer", new[] { "TM_Candidate_TIF_Id" });
            DropIndex("dbo.TM_Candidate_TIF", new[] { "TM_TIF_Status_Id" });
            DropIndex("dbo.TM_Candidate_TIF", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TM_Candidate_TIF", new[] { "Recommended_Rank_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Core", new[] { "TM_MassTIF_Form_Question_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Core", new[] { "TM_Mass_Scoring_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Core", new[] { "TM_Candidate_MassTIF_Id" });
            DropIndex("dbo.TM_MassTIF_Form_Question", new[] { "TM_Mass_TIF_Form_Id" });
            DropIndex("dbo.TM_Mass_Auditing_Question", new[] { "TM_Mass_TIF_Form_Id" });
            DropIndex("dbo.TM_Mass_Auditing_Question", new[] { "TM_Mass_Question_Type_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Audit_Qst", new[] { "TM_Mass_Scoring_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Audit_Qst", new[] { "TM_Mass_Auditing_Question_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Audit_Qst", new[] { "TM_Candidate_MassTIF_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF_Approv", new[] { "TM_Candidate_MassTIF_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF", new[] { "TM_MassTIF_Status_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF", new[] { "TM_Mass_Question_Type_Id" });
            DropIndex("dbo.TM_Candidate_MassTIF", new[] { "Recommended_Rank_Id" });
            DropIndex("dbo.TBtran_Position", new[] { "PRForm_Id" });
            DropIndex("dbo.TBtran_Position", new[] { "EmpType_Id" });
            DropIndex("dbo.TBtran_Candidate", new[] { "PRForm_Id" });
            DropIndex("dbo.TBtran_Candidate", new[] { "InterviewStatus_Id" });
            DropIndex("dbo.TBtran_Candidate", new[] { "HiringProcess_Id" });
            DropIndex("dbo.TBmst_AttachFile", new[] { "PRForm_Id" });
            DropIndex("dbo.TBtran_PRForm", new[] { "Status_Id" });
            DropIndex("dbo.TBtran_PRForm", new[] { "RequestType_Id" });
            DropIndex("dbo.PTR_Evaluation_KPIs", new[] { "TM_PTR_Eva_Questions_Id" });
            DropIndex("dbo.PTR_Evaluation_KPIs", new[] { "PTR_Evaluation_Id" });
            DropIndex("dbo.PTR_Evaluation_File", new[] { "PTR_Evaluation_Id" });
            DropIndex("dbo.TM_PTR_Eva_Score", new[] { "TM_Partner_Evaluation_Id" });
            DropIndex("dbo.TM_PTR_Eva_KPIs", new[] { "TM_Partner_Evaluation_Id" });
            DropIndex("dbo.TM_PTR_Eva_KPIs", new[] { "TM_KPIs_Base_Id" });
            DropIndex("dbo.TM_PTR_Eva_Questions", new[] { "TM_Partner_Evaluation_Id" });
            DropIndex("dbo.PTR_Evaluation_Answer", new[] { "TM_PTR_Eva_Questions_Id" });
            DropIndex("dbo.PTR_Evaluation_Answer", new[] { "PTR_Evaluation_Id" });
            DropIndex("dbo.PTR_Evaluation", new[] { "Self_Annual_Rating_Id" });
            DropIndex("dbo.PTR_Evaluation", new[] { "PTR_Evaluation_Year_Id" });
            DropIndex("dbo.PTR_Evaluation", new[] { "Final_Annual_Rating_Id" });
            DropIndex("dbo.MenuActions", new[] { "Menu_Id" });
            DropIndex("dbo.GroupListPermissions", new[] { "MenuAction_Id" });
            DropIndex("dbo.GroupListPermissions", new[] { "GroupPermission_Id" });
            DropIndex("dbo.TM_TechnicalTestTransaction", new[] { "TM_Candidates_Id" });
            DropIndex("dbo.TM_TechnicalTestTransaction", new[] { "TM_TechnicalTest_Id" });
            DropIndex("dbo.TM_Universitys_Faculty", new[] { "TM_Universitys_Id" });
            DropIndex("dbo.TM_Universitys_Faculty", new[] { "TM_Faculty_Id" });
            DropIndex("dbo.TM_Universitys_Major", new[] { "TM_Universitys_Faculty_Id" });
            DropIndex("dbo.TM_Universitys_Major", new[] { "TM_Major_Id" });
            DropIndex("dbo.TM_Education_History", new[] { "TM_Universitys_Major_Id" });
            DropIndex("dbo.TM_Education_History", new[] { "TM_Education_Degree_Id" });
            DropIndex("dbo.TM_Education_History", new[] { "TM_Candidates_Id" });
            DropIndex("dbo.TM_City", new[] { "TM_Country_Id" });
            DropIndex("dbo.TM_District", new[] { "TM_City_Id" });
            DropIndex("dbo.TM_SubDistrict", new[] { "TM_District_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "TM_SubDistrict_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "TM_SourcingChannel_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "TM_Candidate_Type_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "PA_TM_SubDistrict_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "MaritalStatusName_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "Gender_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "CA_TM_SubDistrict_Id" });
            DropIndex("dbo.TM_Candidate_Status_Next", new[] { "TM_Candidate_Status_Id" });
            DropIndex("dbo.TM_Candidate_Status_Cycle", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TM_Candidate_Status_Cycle", new[] { "TM_Candidate_Status_Id" });
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "TM_Recruitment_Team_Id" });
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "TM_Candidates_Id" });
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "TM_Candidate_Rank_Id" });
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "PersonnelRequest_Id" });
            DropIndex("dbo.TM_Employment_Request", new[] { "TM_Request_Type_Id" });
            DropIndex("dbo.TM_Employment_Request", new[] { "TM_Employment_Type_Id" });
            DropIndex("dbo.TM_UnitGroup_Approve_Permit", new[] { "TM_Divisions_Id" });
            DropIndex("dbo.TM_SubGroup", new[] { "TM_Divisions_Id" });
            DropIndex("dbo.TM_Position", new[] { "TM_Divisions_Id" });
            DropIndex("dbo.TM_Pool_Rank", new[] { "TM_Rank_Id" });
            DropIndex("dbo.TM_Pool_Rank", new[] { "TM_Pool_Id" });
            DropIndex("dbo.TM_Pool_Approve_Permit", new[] { "TM_Pool_Id" });
            DropIndex("dbo.TM_Company_Approve_Permit", new[] { "TM_Company_Id" });
            DropIndex("dbo.TM_Pool", new[] { "TM_Company_Id" });
            DropIndex("dbo.TM_Divisions", new[] { "TM_Pool_Id" });
            DropIndex("dbo.PersonnelRequests", new[] { "TM_SubGroup_Id" });
            DropIndex("dbo.PersonnelRequests", new[] { "TM_PR_Status_Id" });
            DropIndex("dbo.PersonnelRequests", new[] { "TM_Position_Id" });
            DropIndex("dbo.PersonnelRequests", new[] { "TM_Pool_Rank_Id" });
            DropIndex("dbo.PersonnelRequests", new[] { "TM_Employment_Request_Id" });
            DropIndex("dbo.PersonnelRequests", new[] { "TM_Divisions_Id" });
            DropIndex("dbo.TM_MailContent_Param", new[] { "MailContent_Id" });
            DropIndex("dbo.TM_MailContent_Cc_bymail", new[] { "MailContent_Id" });
            DropIndex("dbo.TM_MailContent_Cc", new[] { "MailContent_Id" });
            DropIndex("dbo.E_Mail_History", new[] { "PersonnelRequest_Id" });
            DropIndex("dbo.E_Mail_History", new[] { "MailContent_Id" });
            DropIndex("dbo.TBmst_SubTeamMember", new[] { "SubTeam_Id" });
            DropIndex("dbo.TBmst_SubTeam", new[] { "Division_Id" });
            DropTable("dbo.UserUnitGroups");
            DropTable("dbo.UserPermissions");
            DropTable("dbo.UserListPermissions");
            DropTable("dbo.TraineeMenuActions");
            DropTable("dbo.TraineeMenus");
            DropTable("dbo.TM_Trainee_Eva_Answer");
            DropTable("dbo.TM_Trainee_Eva");
            DropTable("dbo.TM_Step_Approve");
            DropTable("dbo.TM_Flow_Approve");
            DropTable("dbo.TM_Evaluation_Question");
            DropTable("dbo.TM_Evaluation_Form");
            DropTable("dbo.TM_Eva_Rating");
            DropTable("dbo.TM_Employment_Rank");
            DropTable("dbo.TM_TIF_Status");
            DropTable("dbo.TM_Candidate_TIF_Approv");
            DropTable("dbo.TM_TIF_Rating");
            DropTable("dbo.TM_TIF_Form");
            DropTable("dbo.TM_TIF_Form_Question");
            DropTable("dbo.TM_Candidate_TIF_Answer");
            DropTable("dbo.TM_Candidate_TIF");
            DropTable("dbo.TM_MassTIF_Status");
            DropTable("dbo.TM_Candidate_MassTIF_Core");
            DropTable("dbo.TM_Mass_Scoring");
            DropTable("dbo.TM_MassTIF_Form_Question");
            DropTable("dbo.TM_Mass_TIF_Form");
            DropTable("dbo.TM_Mass_Question_Type");
            DropTable("dbo.TM_Mass_Auditing_Question");
            DropTable("dbo.TM_Candidate_MassTIF_Audit_Qst");
            DropTable("dbo.TM_Candidate_MassTIF_Approv");
            DropTable("dbo.TM_Candidate_MassTIF");
            DropTable("dbo.TBmst_RequestType");
            DropTable("dbo.TBmst_EmployeeType");
            DropTable("dbo.TBtran_Position");
            DropTable("dbo.TBmst_InterviewStatus");
            DropTable("dbo.TBmst_HiringProcess");
            DropTable("dbo.TBtran_Candidate");
            DropTable("dbo.TBmst_AttachFile");
            DropTable("dbo.TBtran_PRForm");
            DropTable("dbo.TBmst_Status");
            DropTable("dbo.PTR_Evaluation_Year");
            DropTable("dbo.PTR_Evaluation_KPIs");
            DropTable("dbo.PTR_Evaluation_File");
            DropTable("dbo.TM_PTR_Eva_Score");
            DropTable("dbo.TM_KPIs_Base");
            DropTable("dbo.TM_PTR_Eva_KPIs");
            DropTable("dbo.TM_Partner_Evaluation");
            DropTable("dbo.TM_PTR_Eva_Questions");
            DropTable("dbo.PTR_Evaluation_Answer");
            DropTable("dbo.TM_Annual_Rating");
            DropTable("dbo.PTR_Evaluation");
            DropTable("dbo.TBmst_PreviousFY");
            DropTable("dbo.Menus");
            DropTable("dbo.MenuActions");
            DropTable("dbo.GroupPermissions");
            DropTable("dbo.GroupListPermissions");
            DropTable("dbo.TBmst_FYPlan");
            DropTable("dbo.TM_PR_Status");
            DropTable("dbo.TM_Recruitment_Team");
            DropTable("dbo.TM_TechnicalTest");
            DropTable("dbo.TM_TechnicalTestTransaction");
            DropTable("dbo.TM_SourcingChannel");
            DropTable("dbo.TM_Universitys");
            DropTable("dbo.TM_Faculty");
            DropTable("dbo.TM_Universitys_Faculty");
            DropTable("dbo.TM_Major");
            DropTable("dbo.TM_Universitys_Major");
            DropTable("dbo.TM_Education_Degree");
            DropTable("dbo.TM_Education_History");
            DropTable("dbo.TM_Candidate_Type");
            DropTable("dbo.TM_MaritalStatus");
            DropTable("dbo.TM_Gender");
            DropTable("dbo.TM_Country");
            DropTable("dbo.TM_City");
            DropTable("dbo.TM_District");
            DropTable("dbo.TM_SubDistrict");
            DropTable("dbo.TM_Candidates");
            DropTable("dbo.TM_Candidate_Status_Next");
            DropTable("dbo.TM_Candidate_Status");
            DropTable("dbo.TM_Candidate_Status_Cycle");
            DropTable("dbo.TM_Candidate_Rank");
            DropTable("dbo.TM_PR_Candidate_Mapping");
            DropTable("dbo.TM_Request_Type");
            DropTable("dbo.TM_Employment_Type");
            DropTable("dbo.TM_Employment_Request");
            DropTable("dbo.TM_UnitGroup_Approve_Permit");
            DropTable("dbo.TM_SubGroup");
            DropTable("dbo.TM_Position");
            DropTable("dbo.TM_Rank");
            DropTable("dbo.TM_Pool_Rank");
            DropTable("dbo.TM_Pool_Approve_Permit");
            DropTable("dbo.TM_Company_Approve_Permit");
            DropTable("dbo.TM_Company");
            DropTable("dbo.TM_Pool");
            DropTable("dbo.TM_Divisions");
            DropTable("dbo.PersonnelRequests");
            DropTable("dbo.TM_MailContent_Param");
            DropTable("dbo.TM_MailContent_Cc_bymail");
            DropTable("dbo.TM_MailContent_Cc");
            DropTable("dbo.MailContents");
            DropTable("dbo.E_Mail_History");
            DropTable("dbo.TBmst_SubTeamMember");
            DropTable("dbo.TBmst_SubTeam");
            DropTable("dbo.TBmst_Division");
        }
    }
}
