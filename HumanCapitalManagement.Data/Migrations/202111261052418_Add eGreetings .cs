namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddeGreetings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.eGreetings_Detail",
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
                        eGreetings_Main_Id = c.Int(),
                        TM_eGreetings_Question_Id = c.Int(),
                        Show_Name = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.eGreetings_Main", t => t.eGreetings_Main_Id)
                .ForeignKey("dbo.TM_eGreetings_Question", t => t.TM_eGreetings_Question_Id)
                .Index(t => t.eGreetings_Main_Id)
                .Index(t => t.TM_eGreetings_Question_Id);
            
            CreateTable(
                "dbo.eGreetings_Main",
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
                        TM_eGreetings_Peroid_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_eGreetings_Peroid", t => t.TM_eGreetings_Peroid_Id)
                .Index(t => t.TM_eGreetings_Peroid_Id);
            
            CreateTable(
                "dbo.TM_eGreetings_Peroid",
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
                "dbo.TM_eGreetings_Question",
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
                        TM_eGreetings_Group_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_eGreetings_Group_Question", t => t.TM_eGreetings_Group_Question_Id)
                .Index(t => t.TM_eGreetings_Group_Question_Id);
            
            CreateTable(
                "dbo.TM_eGreetings_Group_Question",
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
            DropForeignKey("dbo.eGreetings_Detail", "TM_eGreetings_Question_Id", "dbo.TM_eGreetings_Question");
            DropForeignKey("dbo.TM_eGreetings_Question", "TM_eGreetings_Group_Question_Id", "dbo.TM_eGreetings_Group_Question");
            DropForeignKey("dbo.eGreetings_Detail", "eGreetings_Main_Id", "dbo.eGreetings_Main");
            DropForeignKey("dbo.eGreetings_Main", "TM_eGreetings_Peroid_Id", "dbo.TM_eGreetings_Peroid");
            DropIndex("dbo.TM_eGreetings_Question", new[] { "TM_eGreetings_Group_Question_Id" });
            DropIndex("dbo.eGreetings_Main", new[] { "TM_eGreetings_Peroid_Id" });
            DropIndex("dbo.eGreetings_Detail", new[] { "TM_eGreetings_Question_Id" });
            DropIndex("dbo.eGreetings_Detail", new[] { "eGreetings_Main_Id" });
            DropTable("dbo.TM_eGreetings_Group_Question");
            DropTable("dbo.TM_eGreetings_Question");
            DropTable("dbo.TM_eGreetings_Peroid");
            DropTable("dbo.eGreetings_Main");
            DropTable("dbo.eGreetings_Detail");
        }
    }
}
