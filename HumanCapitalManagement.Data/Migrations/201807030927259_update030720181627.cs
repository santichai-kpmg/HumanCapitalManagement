namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update030720181627 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PTR_Evaluation_KPIs", "TM_PTR_Eva_Questions_Id", "dbo.TM_PTR_Eva_Questions");
            DropIndex("dbo.PTR_Evaluation_KPIs", new[] { "TM_PTR_Eva_Questions_Id" });
            CreateTable(
                "dbo.PTR_Evaluation_Approve",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Req_Approve_user = c.String(maxLength: 50),
                        Approve_date = c.DateTime(),
                        Approve_user = c.String(maxLength: 50),
                        Approve_status = c.String(maxLength: 10),
                        responses = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_PTR_Eva_ApproveStep_Id = c.Int(),
                        PTR_Evaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_PTR_Eva_ApproveStep", t => t.TM_PTR_Eva_ApproveStep_Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .Index(t => t.TM_PTR_Eva_ApproveStep_Id)
                .Index(t => t.PTR_Evaluation_Id);
            
            AddColumn("dbo.TM_KPIs_Base", "type_of_kpi", c => c.String(maxLength: 10));
            AddColumn("dbo.PTR_Evaluation_KPIs", "target_max", c => c.String(maxLength: 1000));
            AddColumn("dbo.PTR_Evaluation_KPIs", "TM_KPIs_Base_Id", c => c.Int());
            CreateIndex("dbo.PTR_Evaluation_KPIs", "TM_KPIs_Base_Id");
            AddForeignKey("dbo.PTR_Evaluation_KPIs", "TM_KPIs_Base_Id", "dbo.TM_KPIs_Base", "Id");
            DropColumn("dbo.PTR_Evaluation_KPIs", "TM_PTR_Eva_Questions_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PTR_Evaluation_KPIs", "TM_PTR_Eva_Questions_Id", c => c.Int());
            DropForeignKey("dbo.PTR_Evaluation_KPIs", "TM_KPIs_Base_Id", "dbo.TM_KPIs_Base");
            DropForeignKey("dbo.PTR_Evaluation_Approve", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropForeignKey("dbo.PTR_Evaluation_Approve", "TM_PTR_Eva_ApproveStep_Id", "dbo.TM_PTR_Eva_ApproveStep");
            DropIndex("dbo.PTR_Evaluation_KPIs", new[] { "TM_KPIs_Base_Id" });
            DropIndex("dbo.PTR_Evaluation_Approve", new[] { "PTR_Evaluation_Id" });
            DropIndex("dbo.PTR_Evaluation_Approve", new[] { "TM_PTR_Eva_ApproveStep_Id" });
            DropColumn("dbo.PTR_Evaluation_KPIs", "TM_KPIs_Base_Id");
            DropColumn("dbo.PTR_Evaluation_KPIs", "target_max");
            DropColumn("dbo.TM_KPIs_Base", "type_of_kpi");
            DropTable("dbo.PTR_Evaluation_Approve");
            CreateIndex("dbo.PTR_Evaluation_KPIs", "TM_PTR_Eva_Questions_Id");
            AddForeignKey("dbo.PTR_Evaluation_KPIs", "TM_PTR_Eva_Questions_Id", "dbo.TM_PTR_Eva_Questions", "Id");
        }
    }
}
