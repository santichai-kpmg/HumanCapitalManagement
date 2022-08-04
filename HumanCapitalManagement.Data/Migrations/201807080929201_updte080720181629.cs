namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updte080720181629 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PTR_Feedback_Emp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        user_no = c.String(maxLength: 50),
                        user_id = c.String(maxLength: 50),
                        appreciations = c.String(),
                        recommendations = c.String(),
                        PTR_Evaluation_Id = c.Int(),
                        TM_PTR_Eva_Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .ForeignKey("dbo.TM_PTR_Eva_Status", t => t.TM_PTR_Eva_Status_Id)
                .Index(t => t.PTR_Evaluation_Id)
                .Index(t => t.TM_PTR_Eva_Status_Id);
            
            CreateTable(
                "dbo.PTR_Feedback_UnitGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        unitcode = c.String(maxLength: 50),
                        appreciations = c.String(),
                        recommendations = c.String(),
                        PTR_Evaluation_Id = c.Int(),
                        TM_PTR_Eva_Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PTR_Evaluation", t => t.PTR_Evaluation_Id)
                .ForeignKey("dbo.TM_PTR_Eva_Status", t => t.TM_PTR_Eva_Status_Id)
                .Index(t => t.PTR_Evaluation_Id)
                .Index(t => t.TM_PTR_Eva_Status_Id);
            
            CreateTable(
                "dbo.TM_Feedback_Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        feedback_rating_code = c.String(maxLength: 50),
                        feedback_rating_name_th = c.String(maxLength: 250),
                        feedback_rating_name_en = c.String(maxLength: 250),
                        feedback_rating_description = c.String(maxLength: 500),
                        point = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PTR_Evaluation", "other_roles", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PTR_Feedback_UnitGroup", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status");
            DropForeignKey("dbo.PTR_Feedback_UnitGroup", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropForeignKey("dbo.PTR_Feedback_Emp", "TM_PTR_Eva_Status_Id", "dbo.TM_PTR_Eva_Status");
            DropForeignKey("dbo.PTR_Feedback_Emp", "PTR_Evaluation_Id", "dbo.PTR_Evaluation");
            DropIndex("dbo.PTR_Feedback_UnitGroup", new[] { "TM_PTR_Eva_Status_Id" });
            DropIndex("dbo.PTR_Feedback_UnitGroup", new[] { "PTR_Evaluation_Id" });
            DropIndex("dbo.PTR_Feedback_Emp", new[] { "TM_PTR_Eva_Status_Id" });
            DropIndex("dbo.PTR_Feedback_Emp", new[] { "PTR_Evaluation_Id" });
            DropColumn("dbo.PTR_Evaluation", "other_roles");
            DropTable("dbo.TM_Feedback_Rating");
            DropTable("dbo.PTR_Feedback_UnitGroup");
            DropTable("dbo.PTR_Feedback_Emp");
        }
    }
}
