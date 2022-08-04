namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _280520191402 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Perdiem_Transport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        date_start = c.DateTime(),
                        date_end = c.DateTime(),
                        Engagement_Code = c.String(maxLength: 50),
                        remark = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        trainee_create_date = c.DateTime(),
                        trainee_create_user = c.Int(),
                        trainee_update_date = c.DateTime(),
                        trainee_update_user = c.Int(),
                        TM_Time_Type_Id = c.Int(),
                        submit_status = c.String(maxLength: 10),
                        approve_status = c.String(maxLength: 10),
                        Approve_user = c.String(maxLength: 50),
                        Source_Type = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Time_Type", t => t.TM_Time_Type_Id)
                .Index(t => t.TM_Time_Type_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Perdiem_Transport", "TM_Time_Type_Id", "dbo.TM_Time_Type");
            DropIndex("dbo.Perdiem_Transport", new[] { "TM_Time_Type_Id" });
            DropTable("dbo.Perdiem_Transport");
        }
    }
}
