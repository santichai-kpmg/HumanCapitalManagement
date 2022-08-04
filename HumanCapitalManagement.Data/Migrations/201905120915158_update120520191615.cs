namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update120520191615 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PTR_Evaluation_Incidents",
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
                        PTR_Evaluation_Id = c.Int(),
                        TM_PTR_Eva_Incidents_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .ForeignKey("dbo.TM_PTR_Eva_Incidents", t => t.TM_PTR_Eva_Incidents_Id)
                .Index(t => t.PTR_Evaluation_Id)
                .Index(t => t.TM_PTR_Eva_Incidents_Id);
            
            CreateTable(
                "dbo.TM_PTR_Eva_Incidents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        qgroup = c.String(maxLength: 300),
                        header = c.String(maxLength: 300),
                        question = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_Partner_Evaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Partner_Evaluation", t => t.TM_Partner_Evaluation_Id)
                .Index(t => t.TM_Partner_Evaluation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PTR_Evaluation_Incidents", "TM_PTR_Eva_Incidents_Id", "dbo.TM_PTR_Eva_Incidents");
            DropForeignKey("dbo.TM_PTR_Eva_Incidents", "TM_Partner_Evaluation_Id", "dbo.TM_Partner_Evaluation");
            DropForeignKey("dbo.PTR_Evaluation_Incidents", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropIndex("dbo.TM_PTR_Eva_Incidents", new[] { "TM_Partner_Evaluation_Id" });
            DropIndex("dbo.PTR_Evaluation_Incidents", new[] { "TM_PTR_Eva_Incidents_Id" });
            DropIndex("dbo.PTR_Evaluation_Incidents", new[] { "PTR_Evaluation_Id" });
            DropTable("dbo.TM_PTR_Eva_Incidents");
            DropTable("dbo.PTR_Evaluation_Incidents");
        }
    }
}
