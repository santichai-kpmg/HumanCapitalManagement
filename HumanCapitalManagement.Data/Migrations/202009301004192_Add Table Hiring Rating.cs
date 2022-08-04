namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableHiringRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Trainee_HiringRating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Trainee_HiringRating_name_th = c.String(maxLength: 250),
                        Trainee_HiringRating_name_en = c.String(maxLength: 250),
                        Trainee_HiringRating_short_name_th = c.String(maxLength: 250),
                        Trainee_HiringRating_short_name_en = c.String(maxLength: 250),
                        Trainee_HiringRating_description = c.String(maxLength: 500),
                        ceo_approve = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        piority = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TM_Trainee_HiringRating");
        }
    }
}
