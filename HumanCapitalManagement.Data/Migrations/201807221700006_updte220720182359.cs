namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updte220720182359 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Trainee_Eva", "incharge_update_date", c => c.DateTime());
            AddColumn("dbo.TM_Trainee_Eva", "incharge_update_user", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Trainee_Eva", "incharge_update_user");
            DropColumn("dbo.TM_Trainee_Eva", "incharge_update_date");
        }
    }
}
