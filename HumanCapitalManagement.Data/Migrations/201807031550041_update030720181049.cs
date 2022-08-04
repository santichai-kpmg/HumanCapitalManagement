namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update030720181049 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PTR_Evaluation", "Final_Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropForeignKey("dbo.PTR_Evaluation", "Self_Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropIndex("dbo.PTR_Evaluation", new[] { "Final_Annual_Rating_Id" });
            DropIndex("dbo.PTR_Evaluation", new[] { "Self_Annual_Rating_Id" });
            AddColumn("dbo.TM_KPIs_Base", "base_min", c => c.Int());
            AddColumn("dbo.TM_KPIs_Base", "base_max", c => c.Int());
            AddColumn("dbo.PTR_Evaluation_Approve", "Annual_Rating_Id", c => c.Int());
            CreateIndex("dbo.PTR_Evaluation_Approve", "Annual_Rating_Id");
            AddForeignKey("dbo.PTR_Evaluation_Approve", "Annual_Rating_Id", "dbo.TM_Annual_Rating", "Id");
            DropColumn("dbo.PTR_Evaluation", "Final_Annual_Rating_Id");
            DropColumn("dbo.PTR_Evaluation", "Self_Annual_Rating_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PTR_Evaluation", "Self_Annual_Rating_Id", c => c.Int());
            AddColumn("dbo.PTR_Evaluation", "Final_Annual_Rating_Id", c => c.Int());
            DropForeignKey("dbo.PTR_Evaluation_Approve", "Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropIndex("dbo.PTR_Evaluation_Approve", new[] { "Annual_Rating_Id" });
            DropColumn("dbo.PTR_Evaluation_Approve", "Annual_Rating_Id");
            DropColumn("dbo.TM_KPIs_Base", "base_max");
            DropColumn("dbo.TM_KPIs_Base", "base_min");
            CreateIndex("dbo.PTR_Evaluation", "Self_Annual_Rating_Id");
            CreateIndex("dbo.PTR_Evaluation", "Final_Annual_Rating_Id");
            AddForeignKey("dbo.PTR_Evaluation", "Self_Annual_Rating_Id", "dbo.TM_Annual_Rating", "Id");
            AddForeignKey("dbo.PTR_Evaluation", "Final_Annual_Rating_Id", "dbo.TM_Annual_Rating", "Id");
        }
    }
}
