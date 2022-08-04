namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update180720181217 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_TraineeEva_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        eva_status_name_th = c.String(maxLength: 250),
                        eva_status_name_en = c.String(maxLength: 250),
                        eva_short_name_en = c.String(maxLength: 250),
                        eva_status_description = c.String(maxLength: 500),
                        status_id = c.Int(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_Trainee_Eva", "TM_TraineeEva_Status_Id", c => c.Int());
            CreateIndex("dbo.TM_Trainee_Eva", "TM_TraineeEva_Status_Id");
            AddForeignKey("dbo.TM_Trainee_Eva", "TM_TraineeEva_Status_Id", "dbo.TM_TraineeEva_Status", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Trainee_Eva", "TM_TraineeEva_Status_Id", "dbo.TM_TraineeEva_Status");
            DropIndex("dbo.TM_Trainee_Eva", new[] { "TM_TraineeEva_Status_Id" });
            DropColumn("dbo.TM_Trainee_Eva", "TM_TraineeEva_Status_Id");
            DropTable("dbo.TM_TraineeEva_Status");
        }
    }
}
