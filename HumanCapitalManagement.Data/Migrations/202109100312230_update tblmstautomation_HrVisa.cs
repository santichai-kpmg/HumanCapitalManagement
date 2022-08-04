namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetblmstautomation_HrVisa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblMstAutoMails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MailSubject = c.String(maxLength: 500),
                        MailBody = c.String(maxLength: 1000),
                        MailType = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblMstAutoMails");
        }
    }
}
