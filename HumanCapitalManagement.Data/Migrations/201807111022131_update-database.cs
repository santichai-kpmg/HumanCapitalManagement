namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_PTR_E_Mail_History",
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
                        MailContent_Id = c.Int(),
                        PTR_Evaluation_Approve_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailContents", t => t.MailContent_Id)
                .ForeignKey("dbo.PTR_Evaluation_Approve", t => t.PTR_Evaluation_Approve_Id)
                .Index(t => t.MailContent_Id)
                .Index(t => t.PTR_Evaluation_Approve_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_PTR_E_Mail_History", "PTR_Evaluation_Approve_Id", "dbo.PTR_Evaluation_Approve");
            DropForeignKey("dbo.TM_PTR_E_Mail_History", "MailContent_Id", "dbo.MailContents");
            DropIndex("dbo.TM_PTR_E_Mail_History", new[] { "PTR_Evaluation_Approve_Id" });
            DropIndex("dbo.TM_PTR_E_Mail_History", new[] { "MailContent_Id" });
            DropTable("dbo.TM_PTR_E_Mail_History");
        }
    }
}
