namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatenewmoelhiringratinginTM_tranee_Eva : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Trainee_Eva", "TM_Trainee_HiringRating_Id", c => c.Int());
            CreateIndex("dbo.TM_Trainee_Eva", "TM_Trainee_HiringRating_Id");
            AddForeignKey("dbo.TM_Trainee_Eva", "TM_Trainee_HiringRating_Id", "dbo.TM_Trainee_HiringRating", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Trainee_Eva", "TM_Trainee_HiringRating_Id", "dbo.TM_Trainee_HiringRating");
            DropIndex("dbo.TM_Trainee_Eva", new[] { "TM_Trainee_HiringRating_Id" });
            DropColumn("dbo.TM_Trainee_Eva", "TM_Trainee_HiringRating_Id");
        }
    }
}
