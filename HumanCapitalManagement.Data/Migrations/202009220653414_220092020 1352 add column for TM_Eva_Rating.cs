namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2200920201352addcolumnforTM_Eva_Rating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Eva_Rating", "TM_Evaluation_Form_Id", c => c.Int());
            CreateIndex("dbo.TM_Eva_Rating", "TM_Evaluation_Form_Id");
            AddForeignKey("dbo.TM_Eva_Rating", "TM_Evaluation_Form_Id", "dbo.TM_Evaluation_Form", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Eva_Rating", "TM_Evaluation_Form_Id", "dbo.TM_Evaluation_Form");
            DropIndex("dbo.TM_Eva_Rating", new[] { "TM_Evaluation_Form_Id" });
            DropColumn("dbo.TM_Eva_Rating", "TM_Evaluation_Form_Id");
        }
    }
}
