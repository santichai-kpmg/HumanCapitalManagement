namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update241220181815 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PTR_Evaluation_AuthorizedEva",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        authorized_user = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        PTR_Evaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .Index(t => t.PTR_Evaluation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PTR_Evaluation_AuthorizedEva", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropIndex("dbo.PTR_Evaluation_AuthorizedEva", new[] { "PTR_Evaluation_Id" });
            DropTable("dbo.PTR_Evaluation_AuthorizedEva");
        }
    }
}
