namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update120520191815 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_PTR_Eva_Incidents_Score",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        incidents_score_en = c.String(maxLength: 250),
                        incidents_score_short_en = c.String(maxLength: 250),
                        description = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id", c => c.Int());
            CreateIndex("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id");
            AddForeignKey("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id", "dbo.TM_PTR_Eva_Incidents_Score", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id", "dbo.TM_PTR_Eva_Incidents_Score");
            DropIndex("dbo.PTR_Evaluation_Incidents", new[] { "TM_PTR_Eva_Incidents_Score_Id" });
            DropColumn("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id");
            DropTable("dbo.TM_PTR_Eva_Incidents_Score");
        }
    }
}
