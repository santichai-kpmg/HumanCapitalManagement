namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update03071524 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_PTR_Eva_ApproveStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        Step_name_en = c.String(maxLength: 250),
                        Step_short_name_en = c.String(maxLength: 250),
                        Step_description = c.String(maxLength: 500),
                        type_of_step = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PTR_Eva_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        status_name_en = c.String(maxLength: 250),
                        status_short_name_en = c.String(maxLength: 250),
                        status_description = c.String(maxLength: 500),
                        can_edit = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TM_PTR_Eva_Status");
            DropTable("dbo.TM_PTR_Eva_ApproveStep");
        }
    }
}
