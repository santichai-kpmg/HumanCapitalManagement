namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _110920191335 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Probation_Detail",
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
                        Probation_Form_Id = c.Int(),
                        TM_Probation_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Probation_Form", t => t.Probation_Form_Id)
                .ForeignKey("dbo.TM_Probation_Question", t => t.TM_Probation_Question_Id)
                .Index(t => t.Probation_Form_Id)
                .Index(t => t.TM_Probation_Question_Id);
            
            CreateTable(
                "dbo.Probation_Form",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assessment = c.String(maxLength: 2),
                        Remark = c.String(maxLength: 1000),
                        Status = c.String(maxLength: 10),
                        Probation_Active = c.String(maxLength: 10),
                        Provident_Fund = c.String(maxLength: 10),
                        Mail_Send = c.String(maxLength: 10),
                        Start_Pro = c.DateTime(),
                        Count_Date_Pro = c.Int(nullable: false),
                        Staff_No = c.String(maxLength: 10),
                        Staff_Acknowledge_Date = c.DateTime(),
                        PM_No = c.String(maxLength: 10),
                        PM_Submit_Date = c.DateTime(),
                        GroupHead_No = c.String(maxLength: 10),
                        GroupHead_Submit_Date = c.DateTime(),
                        HOP_No = c.String(maxLength: 10),
                        HOP_Submit_Date = c.DateTime(),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Probation_Question",
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Probation_Detail", "TM_Probation_Question_Id", "dbo.TM_Probation_Question");
            DropForeignKey("dbo.Probation_Detail", "Probation_Form_Id", "dbo.Probation_Form");
            DropIndex("dbo.Probation_Detail", new[] { "TM_Probation_Question_Id" });
            DropIndex("dbo.Probation_Detail", new[] { "Probation_Form_Id" });
            DropTable("dbo.TM_Probation_Question");
            DropTable("dbo.Probation_Form");
            DropTable("dbo.Probation_Detail");
        }
    }
}
