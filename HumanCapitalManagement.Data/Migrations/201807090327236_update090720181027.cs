namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update090720181027 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PTR_Feedback_Emp", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status");
            DropForeignKey("dbo.PTR_Feedback_UnitGroup", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status");
            DropIndex("dbo.PTR_Feedback_Emp", new[] { "TM_PTR_Eva_Status_Id" });
            DropIndex("dbo.PTR_Feedback_UnitGroup", new[] { "TM_PTR_Eva_Status_Id" });
            AddColumn("dbo.PTR_Feedback_Emp", "TM_Feedback_Rating_Id", c => c.Int());
            AddColumn("dbo.PTR_Feedback_UnitGroup", "TM_Feedback_Rating_Id", c => c.Int());
            CreateIndex("dbo.PTR_Feedback_Emp", "TM_Feedback_Rating_Id");
            CreateIndex("dbo.PTR_Feedback_UnitGroup", "TM_Feedback_Rating_Id");
            AddForeignKey("dbo.PTR_Feedback_Emp", "TM_Feedback_Rating_Id", "dbo.TM_Feedback_Rating", "Id");
            AddForeignKey("dbo.PTR_Feedback_UnitGroup", "TM_Feedback_Rating_Id", "dbo.TM_Feedback_Rating", "Id");
            DropColumn("dbo.PTR_Feedback_Emp", "TM_PTR_Eva_Status_Id");
            DropColumn("dbo.PTR_Feedback_UnitGroup", "TM_PTR_Eva_Status_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PTR_Feedback_UnitGroup", "TM_PTR_Eva_Status_Id", c => c.Int());
            AddColumn("dbo.PTR_Feedback_Emp", "TM_PTR_Eva_Status_Id", c => c.Int());
            DropForeignKey("dbo.PTR_Feedback_UnitGroup", "TM_Feedback_Rating_Id", "dbo.TM_Feedback_Rating");
            DropForeignKey("dbo.PTR_Feedback_Emp", "TM_Feedback_Rating_Id", "dbo.TM_Feedback_Rating");
            DropIndex("dbo.PTR_Feedback_UnitGroup", new[] { "TM_Feedback_Rating_Id" });
            DropIndex("dbo.PTR_Feedback_Emp", new[] { "TM_Feedback_Rating_Id" });
            DropColumn("dbo.PTR_Feedback_UnitGroup", "TM_Feedback_Rating_Id");
            DropColumn("dbo.PTR_Feedback_Emp", "TM_Feedback_Rating_Id");
            CreateIndex("dbo.PTR_Feedback_UnitGroup", "TM_PTR_Eva_Status_Id");
            CreateIndex("dbo.PTR_Feedback_Emp", "TM_PTR_Eva_Status_Id");
            AddForeignKey("dbo.PTR_Feedback_UnitGroup", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status", "Id");
            AddForeignKey("dbo.PTR_Feedback_Emp", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status", "Id");
        }
    }
}
