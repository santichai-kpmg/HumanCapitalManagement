namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _100720201336addtalbeformdetailandratingforpretrainee : DbMigration
    {
        public override void Up()
        {
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
                        //PreTraineeAssessment_Form_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                //.ForeignKey("dbo.PreTraineeAssessment_Form", t => t.PreTraineeAssessment_Form_Id1)
                .ForeignKey("dbo.PreTraineeAssessment_Form", t => t.PreTraineeAssessment_Form_Id)
                .ForeignKey("dbo.PreTraineeAssessment_Form", t => t.PreTraineeAssessment_Rating_Id)
                .ForeignKey("dbo.TM_PreTraineeAssessment_Question", t => t.TM_PreTraineeAssessment_Question_Id)
                .Index(t => t.PreTraineeAssessment_Form_Id)
                .Index(t => t.TM_PreTraineeAssessment_Question_Id)
                .Index(t => t.PreTraineeAssessment_Rating_Id);
                //.Index(t => t.PreTraineeAssessment_Form_Id1)
            
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
                        PM_No = c.String(maxLength: 10),
                        PM_Submit_Date = c.DateTime(),
                        GroupHead_No = c.String(maxLength: 10),
                        GroupHead_Submit_Date = c.DateTime(),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Remark_Revise = c.String(maxLength: 1000),
                        Send_Mail_Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TM_PreTraineeAssessment_Question",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Seq = c.Int(nullable: false),
            //            Topic = c.String(maxLength: 1000),
            //            Content = c.String(),
            //            Use = c.String(maxLength: 10),
            //            Active_Status = c.String(maxLength: 10),
            //            Create_Date = c.DateTime(),
            //            Create_User = c.String(maxLength: 10),
            //            Update_Date = c.DateTime(),
            //            Update_User = c.String(maxLength: 10),
            //            Icon = c.String(),
            //            TM_PreTraineeAssessment_Group_Id = c.Int(),
            //            TM_PreTraineeAssessment_Group_Question_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.TM_PreTraineeAssessment_Group", t => t.TM_PreTraineeAssessment_Group_Id)
            //    .ForeignKey("dbo.TM_PreTraineeAssessment_Group_Question", t => t.TM_PreTraineeAssessment_Group_Question_Id)
            //    .Index(t => t.TM_PreTraineeAssessment_Group_Id)
            //    .Index(t => t.TM_PreTraineeAssessment_Group_Question_Id);
            
            //CreateTable(
            //    "dbo.TM_PreTraineeAssessment_Group",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(maxLength: 50),
            //            Active_Status = c.String(maxLength: 10),
            //            Action_date = c.DateTime(),
            //            Description = c.String(maxLength: 500),
            //            Create_Date = c.DateTime(),
            //            Create_User = c.String(maxLength: 10),
            //            Update_Date = c.DateTime(),
            //            Update_User = c.String(maxLength: 10),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TM_PreTraineeAssessment_Group_Question",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(maxLength: 50),
            //            Active_Status = c.String(maxLength: 10),
            //            Action_date = c.DateTime(),
            //            Description = c.String(maxLength: 500),
            //            Seq = c.String(),
            //            Create_Date = c.DateTime(),
            //            Create_User = c.String(maxLength: 10),
            //            Update_Date = c.DateTime(),
            //            Update_User = c.String(maxLength: 10),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreTraineeAssessment_Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 1000),
                        Remark = c.String(maxLength: 1000),
                        Type = c.String(maxLength: 10),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreTraineeAssessment_Details", "TM_PreTraineeAssessment_Question_Id", "dbo.TM_PreTraineeAssessment_Question");
            //DropForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Question_Id", "dbo.TM_PreTraineeAssessment_Group_Question");
            //DropForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Id", "dbo.TM_PreTraineeAssessment_Group");
            //DropForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Rating_Id", "dbo.PreTraineeAssessment_Form");
            //DropForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id", "dbo.PreTraineeAssessment_Form");
            //DropForeignKey("dbo.PreTraineeAssessment_Details", "PreTraineeAssessment_Form_Id1", "dbo.PreTraineeAssessment_Form");
            //DropIndex("dbo.TM_PreTraineeAssessment_Question", new[] { "TM_PreTraineeAssessment_Group_Question_Id" });
            //DropIndex("dbo.TM_PreTraineeAssessment_Question", new[] { "TM_PreTraineeAssessment_Group_Id" });
            //DropIndex("dbo.PreTraineeAssessment_Details", new[] { "PreTraineeAssessment_Form_Id1" });
            DropIndex("dbo.PreTraineeAssessment_Details", new[] { "PreTraineeAssessment_Rating_Id" });
            DropIndex("dbo.PreTraineeAssessment_Details", new[] { "TM_PreTraineeAssessment_Question_Id" });
            DropIndex("dbo.PreTraineeAssessment_Details", new[] { "PreTraineeAssessment_Form_Id" });
            DropTable("dbo.PreTraineeAssessment_Rating");
            //DropTable("dbo.TM_PreTraineeAssessment_Group_Question");
            //DropTable("dbo.TM_PreTraineeAssessment_Group");
            //DropTable("dbo.TM_PreTraineeAssessment_Question");
            DropTable("dbo.PreTraineeAssessment_Form");
            DropTable("dbo.PreTraineeAssessment_Details");
        }
    }
}
