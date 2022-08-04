namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _200120211108addminiheart2021 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MiniHeart_Detail2021",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rank = c.String(maxLength: 2),
                        Emp_No = c.String(maxLength: 10),
                        Reason = c.String(maxLength: 1000),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        MiniHeart_Main_Id = c.Int(),
                        TM_MiniHeart_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MiniHeart_Main2021", t => t.MiniHeart_Main_Id)
                .ForeignKey("dbo.TM_MiniHeart_Question2021", t => t.TM_MiniHeart_Question_Id)
                .Index(t => t.MiniHeart_Main_Id)
                .Index(t => t.TM_MiniHeart_Question_Id);
            
            CreateTable(
                "dbo.MiniHeart_Main2021",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .ForeignKey("dbo.TM_MiniHeart_Peroid2021", t => t.TM_MiniHeart_Peroid_Id)
                .Index(t => t.TM_MiniHeart_Peroid_Id);
            
            CreateTable(
                "dbo.TM_MiniHeart_Peroid2021",
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
            
            CreateTable(
                "dbo.TM_MiniHeart_Question2021",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seq = c.Int(nullable: false),
                        Group = c.Int(nullable: false),
                        Topic = c.String(maxLength: 1000),
                        Content = c.String(),
                        Use = c.String(maxLength: 10),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Icon = c.String(),
                        TM_MiniHeart_Group_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_MiniHeart_Group_Question2021", t => t.TM_MiniHeart_Group_Question_Id)
                .Index(t => t.TM_MiniHeart_Group_Question_Id);
            
            CreateTable(
                "dbo.TM_MiniHeart_Group_Question2021",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MiniHeart_Detail2021", "TM_MiniHeart_Question_Id", "dbo.TM_MiniHeart_Question2021");
            DropForeignKey("dbo.TM_MiniHeart_Question2021", "TM_MiniHeart_Group_Question_Id", "dbo.TM_MiniHeart_Group_Question2021");
            DropForeignKey("dbo.MiniHeart_Detail2021", "MiniHeart_Main_Id", "dbo.MiniHeart_Main2021");
            DropForeignKey("dbo.MiniHeart_Main2021", "TM_MiniHeart_Peroid_Id", "dbo.TM_MiniHeart_Peroid2021");
            DropIndex("dbo.TM_MiniHeart_Question2021", new[] { "TM_MiniHeart_Group_Question_Id" });
            DropIndex("dbo.MiniHeart_Main2021", new[] { "TM_MiniHeart_Peroid_Id" });
            DropIndex("dbo.MiniHeart_Detail2021", new[] { "TM_MiniHeart_Question_Id" });
            DropIndex("dbo.MiniHeart_Detail2021", new[] { "MiniHeart_Main_Id" });
            DropTable("dbo.TM_MiniHeart_Group_Question2021");
            DropTable("dbo.TM_MiniHeart_Question2021");
            DropTable("dbo.TM_MiniHeart_Peroid2021");
            DropTable("dbo.MiniHeart_Main2021");
            DropTable("dbo.MiniHeart_Detail2021");
        }
    }
}
