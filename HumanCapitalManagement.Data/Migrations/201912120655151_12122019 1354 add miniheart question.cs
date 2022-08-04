namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _121220191354addminiheartquestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_MiniHeart_Group_Question",
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
            
            CreateTable(
                "dbo.TM_MiniHeart_Question",
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
                .ForeignKey("dbo.TM_MiniHeart_Group_Question", t => t.TM_MiniHeart_Group_Question_Id)
                .Index(t => t.TM_MiniHeart_Group_Question_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_MiniHeart_Question", "TM_MiniHeart_Group_Question_Id", "dbo.TM_MiniHeart_Group_Question");
            DropIndex("dbo.TM_MiniHeart_Question", new[] { "TM_MiniHeart_Group_Question_Id" });
            DropTable("dbo.TM_MiniHeart_Question");
            DropTable("dbo.TM_MiniHeart_Group_Question");
        }
    }
}
