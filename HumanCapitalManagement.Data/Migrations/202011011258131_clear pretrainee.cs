namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clearpretrainee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities");
            DropForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id1", "dbo.PreTraineeAssessment_Form");
            DropForeignKey("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping");
            DropForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id", "dbo.PreTraineeAssessment_Form");
            DropForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Rating_Id", "dbo.PreTraineeAssessment_Form");
            DropForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Id", "dbo.TM_PreTraineeAssessment_Group");
            DropForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Question_Id", "dbo.TM_PreTraineeAssessment_Group_Question");
            DropForeignKey("dbo.PreTraineeAssessment_Details", "TM_PreTraineeAssessment_Question_Id", "dbo.TM_PreTraineeAssessment_Question");
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "TM_PreTraineeAssessment_Activities_Id" });
            DropIndex("dbo.PreTraineeAssessment_Details", new[] { "PreTraineeAssessment_Form_Id" });
            DropIndex("dbo.PreTraineeAssessment_Details", new[] { "TM_PreTraineeAssessment_Question_Id" });
            DropIndex("dbo.PreTraineeAssessment_Details", new[] { "PreTraineeAssessment_Rating_Id" });
            DropIndex("dbo.PreTraineeAssessment_Details", new[] { "PreTraineeAssessment_Form_Id1" });
            DropIndex("dbo.PreTraineeAssessment_Form", new[] { "TM_PR_Candidate_Mapping_Id" });
            DropIndex("dbo.TM_PreTraineeAssessment_Question", new[] { "TM_PreTraineeAssessment_Group_Id" });
            DropIndex("dbo.TM_PreTraineeAssessment_Question", new[] { "TM_PreTraineeAssessment_Group_Question_Id" });
            AddColumn("dbo.TM_PR_Candidate_Mapping", "TM_PInternAssessment_Activities_Id", c => c.Int());
            CreateIndex("dbo.TM_PR_Candidate_Mapping", "TM_PInternAssessment_Activities_Id");
            AddForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_PInternAssessment_Activities_Id", "dbo.TM_PInternAssessment_Activities", "Id");
            DropColumn("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id");
            DropTable("dbo.TM_PreTraineeAssessment_Activities");
            DropTable("dbo.PreTraineeAssessment_Details");
            DropTable("dbo.PreTraineeAssessment_Form");
            DropTable("dbo.TM_PreTraineeAssessment_Question");
            DropTable("dbo.TM_PreTraineeAssessment_Group");
            DropTable("dbo.TM_PreTraineeAssessment_Group_Question");
            DropTable("dbo.TM_PreTraineeAssessment_Rating");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TM_PreTraineeAssessment_Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 1000),
                        Remark = c.String(maxLength: 1000),
                        Type = c.String(maxLength: 10),
                        Seq = c.String(maxLength: 10),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PreTraineeAssessment_Group_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active_Status = c.String(maxLength: 10),
                        Action_date = c.DateTime(),
                        Description = c.String(maxLength: 500),
                        Seq = c.String(),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PreTraineeAssessment_Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Active_Status = c.String(maxLength: 10),
                        Action_date = c.DateTime(),
                        Description = c.String(maxLength: 500),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PreTraineeAssessment_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seq = c.Int(nullable: false),
                        Topic = c.String(maxLength: 1000),
                        Content = c.String(),
                        Use = c.String(maxLength: 10),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Icon = c.String(),
                        TM_PreTraineeAssessment_Group_Id = c.Int(),
                        TM_PreTraineeAssessment_Group_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreTraineeAssessment_Form",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assessment = c.String(maxLength: 2),
                        Remark = c.String(maxLength: 1000),
                        recognize = c.String(maxLength: 10),
                        Status = c.String(maxLength: 10),
                        Mail_Send = c.String(maxLength: 10),
                        Trainee_No = c.String(maxLength: 10),
                        HR_No = c.String(maxLength: 10),
                        HR_Submit_Date = c.DateTime(),
                        First_No = c.String(maxLength: 10),
                        First_Submit_Date = c.DateTime(),
                        Second_No = c.String(maxLength: 10),
                        Second_Submit_Date = c.DateTime(),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Remark_Revise = c.String(maxLength: 1000),
                        Send_Mail_Date = c.DateTime(),
                        TM_PR_Candidate_Mapping_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreTraineeAssessment_Details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seq = c.Int(nullable: false),
                        Assessment = c.String(maxLength: 2),
                        Remark = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        PreTraineeAssessment_Form_Id = c.Int(),
                        TM_PreTraineeAssessment_Question_Id = c.Int(),
                        PreTraineeAssessment_Rating_Id = c.Int(),
                        PreTraineeAssessment_Form_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PreTraineeAssessment_Activities",
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
            
            AddColumn("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id", c => c.Int());
            DropForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_PInternAssessment_Activities_Id", "dbo.TM_PInternAssessment_Activities");
            DropIndex("dbo.TM_PR_Candidate_Mapping", new[] { "TM_PInternAssessment_Activities_Id" });
            DropColumn("dbo.TM_PR_Candidate_Mapping", "TM_PInternAssessment_Activities_Id");
            CreateIndex("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Question_Id");
            CreateIndex("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Id");
            CreateIndex("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id");
            CreateIndex("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id1");
            CreateIndex("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Rating_Id");
            CreateIndex("dbo.PreTraineeAssessment_Details", "TM_PreTraineeAssessment_Question_Id");
            CreateIndex("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id");
            CreateIndex("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id");
            AddForeignKey("dbo.PreTraineeAssessment_Details", "TM_PreTraineeAssessment_Question_Id", "dbo.TM_PreTraineeAssessment_Question", "Id");
            AddForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Question_Id", "dbo.TM_PreTraineeAssessment_Group_Question", "Id");
            AddForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Id", "dbo.TM_PreTraineeAssessment_Group", "Id");
            AddForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Rating_Id", "dbo.PreTraineeAssessment_Form", "Id");
            AddForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id", "dbo.PreTraineeAssessment_Form", "Id");
            AddForeignKey("dbo.PreTraineeAssessment_Form", "TM_PR_Candidate_Mapping_Id", "dbo.TM_PR_Candidate_Mapping", "Id");
            AddForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id1", "dbo.PreTraineeAssessment_Form", "Id");
            AddForeignKey("dbo.TM_PR_Candidate_Mapping", "TM_PreTraineeAssessment_Activities_Id", "dbo.TM_PreTraineeAssessment_Activities", "Id");
        }
    }
}
