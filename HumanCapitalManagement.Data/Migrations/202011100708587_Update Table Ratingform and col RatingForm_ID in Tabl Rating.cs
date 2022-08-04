namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableRatingformandcolRatingForm_IDinTablRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_PIntern_RatingForm",
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
            
            AddColumn("dbo.TM_PIntern_Rating", "TM_PIntern_RatingForm_Id", c => c.Int());
            CreateIndex("dbo.TM_PIntern_Rating", "TM_PIntern_RatingForm_Id");
            AddForeignKey("dbo.TM_PIntern_Rating", "TM_PIntern_RatingForm_Id", "dbo.TM_PIntern_RatingForm", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_PIntern_Rating", "TM_PIntern_RatingForm_Id", "dbo.TM_PIntern_RatingForm");
            DropIndex("dbo.TM_PIntern_Rating", new[] { "TM_PIntern_RatingForm_Id" });
            DropColumn("dbo.TM_PIntern_Rating", "TM_PIntern_RatingForm_Id");
            DropTable("dbo.TM_PIntern_RatingForm");
        }
    }
}
