namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addconsenttosolution : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Consent_Asnwer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asnwer = c.String(maxLength: 2),
                        Description = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Consent_Main_Form_Id = c.Int(),
                        TM_Consent_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Consent_Main_Form", t => t.Consent_Main_Form_Id)
                .ForeignKey("dbo.TM_Consent_Question", t => t.TM_Consent_Question_Id)
                .Index(t => t.Consent_Main_Form_Id)
                .Index(t => t.TM_Consent_Question_Id);
            
            CreateTable(
                "dbo.Consent_Main_Form",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 1000),
                        Description = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        TM_Consent_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Consent_Form", t => t.TM_Consent_Form_Id)
                .Index(t => t.TM_Consent_Form_Id);
            
            CreateTable(
                "dbo.TM_Consent_Form",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 1000),
                        Description = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Consent_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seq = c.Int(nullable: false),
                        Type = c.String(maxLength: 2),
                        Topic = c.String(maxLength: 1000),
                        Content = c.String(),
                        Description = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        TM_Consent_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Consent_Form", t => t.TM_Consent_Form_Id)
                .Index(t => t.TM_Consent_Form_Id);
            
            CreateTable(
                "dbo.Consent_Asnwer_Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asnwer = c.String(maxLength: 2),
                        Description = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Consent_Main_Form_Id = c.Int(),
                        TM_Consent_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Consent_Main_Form", t => t.Consent_Main_Form_Id)
                .ForeignKey("dbo.TM_Consent_Question", t => t.TM_Consent_Question_Id)
                .Index(t => t.Consent_Main_Form_Id)
                .Index(t => t.TM_Consent_Question_Id);
            
            CreateTable(
                "dbo.Consent_Spacial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeNo = c.String(maxLength: 1000),
                        Description = c.String(maxLength: 1000),
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
            DropForeignKey("dbo.Consent_Asnwer_Log", "TM_Consent_Question_Id", "dbo.TM_Consent_Question");
            DropForeignKey("dbo.Consent_Asnwer_Log", "Consent_Main_Form_Id", "dbo.Consent_Main_Form");
            DropForeignKey("dbo.Consent_Asnwer", "TM_Consent_Question_Id", "dbo.TM_Consent_Question");
            DropForeignKey("dbo.Consent_Main_Form", "TM_Consent_Form_Id", "dbo.TM_Consent_Form");
            DropForeignKey("dbo.TM_Consent_Question", "TM_Consent_Form_Id", "dbo.TM_Consent_Form");
            DropForeignKey("dbo.Consent_Asnwer", "Consent_Main_Form_Id", "dbo.Consent_Main_Form");
            DropIndex("dbo.Consent_Asnwer_Log", new[] { "TM_Consent_Question_Id" });
            DropIndex("dbo.Consent_Asnwer_Log", new[] { "Consent_Main_Form_Id" });
            DropIndex("dbo.TM_Consent_Question", new[] { "TM_Consent_Form_Id" });
            DropIndex("dbo.Consent_Main_Form", new[] { "TM_Consent_Form_Id" });
            DropIndex("dbo.Consent_Asnwer", new[] { "TM_Consent_Question_Id" });
            DropIndex("dbo.Consent_Asnwer", new[] { "Consent_Main_Form_Id" });
            DropTable("dbo.Consent_Spacial");
            DropTable("dbo.Consent_Asnwer_Log");
            DropTable("dbo.TM_Consent_Question");
            DropTable("dbo.TM_Consent_Form");
            DropTable("dbo.Consent_Main_Form");
            DropTable("dbo.Consent_Asnwer");
        }
    }
}
