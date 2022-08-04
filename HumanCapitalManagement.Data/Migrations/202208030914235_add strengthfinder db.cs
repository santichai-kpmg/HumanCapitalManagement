namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstrengthfinderdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_StrengthFinderHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeNo = c.String(),
                        UpdateUser = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        StrengthName1_Id = c.Int(),
                        StrengthName2_Id = c.Int(),
                        StrengthName3_Id = c.Int(),
                        StrengthName4_Id = c.Int(),
                        StrengthName5_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_StrenghtFinder", t => t.StrengthName1_Id)
                .ForeignKey("dbo.TM_StrenghtFinder", t => t.StrengthName2_Id)
                .ForeignKey("dbo.TM_StrenghtFinder", t => t.StrengthName3_Id)
                .ForeignKey("dbo.TM_StrenghtFinder", t => t.StrengthName4_Id)
                .ForeignKey("dbo.TM_StrenghtFinder", t => t.StrengthName5_Id)
                .Index(t => t.StrengthName1_Id)
                .Index(t => t.StrengthName2_Id)
                .Index(t => t.StrengthName3_Id)
                .Index(t => t.StrengthName4_Id)
                .Index(t => t.StrengthName5_Id);
            
            CreateTable(
                "dbo.TM_StrenghtFinder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StrengthName = c.String(maxLength: 250),
                        CreateDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_StrengthFinderHistory", "StrengthName5_Id", "dbo.TM_StrenghtFinder");
            DropForeignKey("dbo.TB_StrengthFinderHistory", "StrengthName4_Id", "dbo.TM_StrenghtFinder");
            DropForeignKey("dbo.TB_StrengthFinderHistory", "StrengthName3_Id", "dbo.TM_StrenghtFinder");
            DropForeignKey("dbo.TB_StrengthFinderHistory", "StrengthName2_Id", "dbo.TM_StrenghtFinder");
            DropForeignKey("dbo.TB_StrengthFinderHistory", "StrengthName1_Id", "dbo.TM_StrenghtFinder");
            DropIndex("dbo.TB_StrengthFinderHistory", new[] { "StrengthName5_Id" });
            DropIndex("dbo.TB_StrengthFinderHistory", new[] { "StrengthName4_Id" });
            DropIndex("dbo.TB_StrengthFinderHistory", new[] { "StrengthName3_Id" });
            DropIndex("dbo.TB_StrengthFinderHistory", new[] { "StrengthName2_Id" });
            DropIndex("dbo.TB_StrengthFinderHistory", new[] { "StrengthName1_Id" });
            DropTable("dbo.TM_StrenghtFinder");
            DropTable("dbo.TB_StrengthFinderHistory");
        }
    }
}
