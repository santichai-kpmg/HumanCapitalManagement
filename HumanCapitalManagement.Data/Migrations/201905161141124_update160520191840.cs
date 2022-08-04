namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update160520191840 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id", "dbo.TM_PTR_Eva_Incidents_Score");
            DropIndex("dbo.PTR_Evaluation_Incidents", new[] { "TM_PTR_Eva_Incidents_Score_Id" });
            CreateTable(
                "dbo.PTR_Evaluation_Incidents_Score",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        answer = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_PTR_Eva_Incidents_Score_Id = c.Int(),
                        PTR_Evaluation_Approve_Id = c.Int(),
                        TM_PTR_Eva_Incidents_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation_Approve", t => t.PTR_Evaluation_Approve_Id)
                .ForeignKey("dbo.TM_PTR_Eva_Incidents", t => t.TM_PTR_Eva_Incidents_Id)
                .ForeignKey("dbo.TM_PTR_Eva_Incidents_Score", t => t.TM_PTR_Eva_Incidents_Score_Id)
                .Index(t => t.TM_PTR_Eva_Incidents_Score_Id)
                .Index(t => t.PTR_Evaluation_Approve_Id)
                .Index(t => t.TM_PTR_Eva_Incidents_Id);
            
            DropColumn("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id", c => c.Int());
            DropForeignKey("dbo.PTR_Evaluation_Incidents_Score", "TM_PTR_Eva_Incidents_Score_Id", "dbo.TM_PTR_Eva_Incidents_Score");
            DropForeignKey("dbo.PTR_Evaluation_Incidents_Score", "TM_PTR_Eva_Incidents_Id", "dbo.TM_PTR_Eva_Incidents");
            DropForeignKey("dbo.PTR_Evaluation_Incidents_Score", "PTR_Evaluation_Approve_Id", "dbo.PTR_Evaluation_Approve");
            DropIndex("dbo.PTR_Evaluation_Incidents_Score", new[] { "TM_PTR_Eva_Incidents_Id" });
            DropIndex("dbo.PTR_Evaluation_Incidents_Score", new[] { "PTR_Evaluation_Approve_Id" });
            DropIndex("dbo.PTR_Evaluation_Incidents_Score", new[] { "TM_PTR_Eva_Incidents_Score_Id" });
            DropTable("dbo.PTR_Evaluation_Incidents_Score");
            CreateIndex("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id");
            AddForeignKey("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Score_Id", "dbo.TM_PTR_Eva_Incidents_Score", "Id");
        }
    }
}
