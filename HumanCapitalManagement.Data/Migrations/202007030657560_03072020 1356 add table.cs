namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _030720201356addtable : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_PreTraineeAssessment_Group", t => t.TM_PreTraineeAssessment_Group_Id)
                .ForeignKey("dbo.TM_PreTraineeAssessment_Group_Question", t => t.TM_PreTraineeAssessment_Group_Question_Id)
                .Index(t => t.TM_PreTraineeAssessment_Group_Id)
                .Index(t => t.TM_PreTraineeAssessment_Group_Question_Id);
            
            CreateTable(
                "dbo.TM_PreTraineeAssessment_Group_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Question_Id", "dbo.TM_PreTraineeAssessment_Group_Question");
            DropForeignKey("dbo.TM_PreTraineeAssessment_Question", "TM_PreTraineeAssessment_Group_Id", "dbo.TM_PreTraineeAssessment_Group");
            DropIndex("dbo.TM_PreTraineeAssessment_Question", new[] { "TM_PreTraineeAssessment_Group_Question_Id" });
            DropIndex("dbo.TM_PreTraineeAssessment_Question", new[] { "TM_PreTraineeAssessment_Group_Id" });
            DropTable("dbo.TM_PreTraineeAssessment_Group_Question");
            DropTable("dbo.TM_PreTraineeAssessment_Question");
            DropTable("dbo.TM_PreTraineeAssessment_Group");
        }
    }
}
