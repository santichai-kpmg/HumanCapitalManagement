namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update050620191125 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PES_Final_Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        Self_TM_Annual_Rating_Id = c.Int(),
                        Final_TM_Annual_Rating_Id = c.Int(),
                        PES_Final_Rating_Year_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Annual_Rating", t => t.Final_TM_Annual_Rating_Id)
                .ForeignKey("dbo.PES_Final_Rating_Year", t => t.PES_Final_Rating_Year_Id)
                .ForeignKey("dbo.TM_Annual_Rating", t => t.Self_TM_Annual_Rating_Id)
                .Index(t => t.Self_TM_Annual_Rating_Id)
                .Index(t => t.Final_TM_Annual_Rating_Id)
                .Index(t => t.PES_Final_Rating_Year_Id);
            
            CreateTable(
                "dbo.PES_Final_Rating_Year",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        evaluation_year = c.DateTime(),
                        active_status = c.String(maxLength: 10),
                        comments = c.String(maxLength: 1500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PES_Final_Rating", "Self_TM_Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropForeignKey("dbo.PES_Final_Rating", "PES_Final_Rating_Year_Id", "dbo.PES_Final_Rating_Year");
            DropForeignKey("dbo.PES_Final_Rating", "Final_TM_Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropIndex("dbo.PES_Final_Rating", new[] { "PES_Final_Rating_Year_Id" });
            DropIndex("dbo.PES_Final_Rating", new[] { "Final_TM_Annual_Rating_Id" });
            DropIndex("dbo.PES_Final_Rating", new[] { "Self_TM_Annual_Rating_Id" });
            DropTable("dbo.PES_Final_Rating_Year");
            DropTable("dbo.PES_Final_Rating");
        }
    }
}
