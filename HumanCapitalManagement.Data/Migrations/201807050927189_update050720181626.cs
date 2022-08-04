namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update050720181626 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PTR_Evaluation", "TM_PTR_Eva_Status_Id", c => c.Int());
            CreateIndex("dbo.PTR_Evaluation", "TM_PTR_Eva_Status_Id");
            AddForeignKey("dbo.PTR_Evaluation", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PTR_Evaluation", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status");
            DropIndex("dbo.PTR_Evaluation", new[] { "TM_PTR_Eva_Status_Id" });
            DropColumn("dbo.PTR_Evaluation", "TM_PTR_Eva_Status_Id");
        }
    }
}
