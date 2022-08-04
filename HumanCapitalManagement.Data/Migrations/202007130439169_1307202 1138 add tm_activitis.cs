namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13072021138addtm_activitis : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Activities_name_en = c.String(maxLength: 250),
                        Activities_name_th = c.String(maxLength: 250),
                        Activities_short_name_en = c.String(maxLength: 250),
                        Activities_short_name_th = c.String(maxLength: 250),
                        Activities_descriptions = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TM_Activities");
        }
    }
}
