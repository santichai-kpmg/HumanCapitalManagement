namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update100820181423 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Additional_Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        answers = c.String(maxLength: 1000),
                        is_other = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Additional_Questions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Additional_Questions", t => t.TM_Additional_Questions_Id)
                .Index(t => t.TM_Additional_Questions_Id);
            
            CreateTable(
                "dbo.TM_Additional_Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        header = c.String(maxLength: 1000),
                        question = c.String(maxLength: 1000),
                        multi_answer = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Additional_Information_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Additional_Information", t => t.TM_Additional_Information_Id)
                .Index(t => t.TM_Additional_Information_Id);
            
            CreateTable(
                "dbo.TM_Additional_Information",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Additional_Questions", "TM_Additional_Information_Id", "dbo.TM_Additional_Information");
            DropForeignKey("dbo.TM_Additional_Answers", "TM_Additional_Questions_Id", "dbo.TM_Additional_Questions");
            DropIndex("dbo.TM_Additional_Questions", new[] { "TM_Additional_Information_Id" });
            DropIndex("dbo.TM_Additional_Answers", new[] { "TM_Additional_Questions_Id" });
            DropTable("dbo.TM_Additional_Information");
            DropTable("dbo.TM_Additional_Questions");
            DropTable("dbo.TM_Additional_Answers");
        }
    }
}
