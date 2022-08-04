namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update120920181831 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogPersonnelRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonnelRequest_Id = c.Int(),
                        seq = c.Int(),
                        TM_Divisions_Id = c.Int(),
                        TM_SubGroup_Id = c.Int(),
                        TM_Employment_Request_Id = c.Int(),
                        TM_Pool_Rank_Id = c.Int(),
                        TM_Position_Id = c.Int(),
                        TM_PR_Status_Id = c.Int(),
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogPersonnelRequests");
        }
    }
}
