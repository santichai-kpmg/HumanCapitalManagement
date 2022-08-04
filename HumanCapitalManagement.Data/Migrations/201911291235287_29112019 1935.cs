namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _291120191935 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MiniHeart_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rank = c.String(maxLength: 2),
                        Emp_Id = c.String(maxLength: 2),
                        Reason = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        MiniHeart_Main_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MiniHeart_Main", t => t.MiniHeart_Main_Id)
                .Index(t => t.MiniHeart_Main_Id);
            
            CreateTable(
                "dbo.MiniHeart_Main",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start_Peroid = c.DateTime(),
                        End_Peroid = c.DateTime(),
                        Emp_No = c.String(maxLength: 10),
                        Remaining_Rights = c.Int(nullable: false),
                        Status = c.String(maxLength: 10),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Remark = c.String(maxLength: 1000),
                        TM_MiniHeart_Peroid_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_MiniHeart_Peroid", t => t.TM_MiniHeart_Peroid_Id)
                .Index(t => t.TM_MiniHeart_Peroid_Id);
            
            CreateTable(
                "dbo.TM_MiniHeart_Peroid",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start_Peroid = c.DateTime(),
                        End_Peroid = c.DateTime(),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Remark = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MiniHeart_Detail", "MiniHeart_Main_Id", "dbo.MiniHeart_Main");
            DropForeignKey("dbo.MiniHeart_Main", "TM_MiniHeart_Peroid_Id", "dbo.TM_MiniHeart_Peroid");
            DropIndex("dbo.MiniHeart_Main", new[] { "TM_MiniHeart_Peroid_Id" });
            DropIndex("dbo.MiniHeart_Detail", new[] { "MiniHeart_Main_Id" });
            DropTable("dbo.TM_MiniHeart_Peroid");
            DropTable("dbo.MiniHeart_Main");
            DropTable("dbo.MiniHeart_Detail");
        }
    }
}
