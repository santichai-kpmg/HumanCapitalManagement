namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _131220191631 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MiniHeart_Detail", "TM_MiniHeart_Question_Id", c => c.Int());
            CreateIndex("dbo.MiniHeart_Detail", "TM_MiniHeart_Question_Id");
            AddForeignKey("dbo.MiniHeart_Detail", "TM_MiniHeart_Question_Id", "dbo.TM_MiniHeart_Question", "Id");
            DropColumn("dbo.MiniHeart_Detail", "Group");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MiniHeart_Detail", "Group", c => c.Int(nullable: false));
            DropForeignKey("dbo.MiniHeart_Detail", "TM_MiniHeart_Question_Id", "dbo.TM_MiniHeart_Question");
            DropIndex("dbo.MiniHeart_Detail", new[] { "TM_MiniHeart_Question_Id" });
            DropColumn("dbo.MiniHeart_Detail", "TM_MiniHeart_Question_Id");
        }
    }
}
