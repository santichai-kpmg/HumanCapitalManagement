namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update160720181612 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Trainee_Eva", "trainee_learned", c => c.String(maxLength: 2500));
            AddColumn("dbo.TM_Trainee_Eva", "trainee_done_well", c => c.String(maxLength: 2500));
            AddColumn("dbo.TM_Trainee_Eva", "trainee_developmental", c => c.String(maxLength: 2500));
            AlterColumn("dbo.TM_Trainee_Eva", "incharge_comments", c => c.String(maxLength: 2500));
            DropColumn("dbo.TM_Trainee_Eva", "trainee_comments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_Trainee_Eva", "trainee_comments", c => c.String(maxLength: 1500));
            AlterColumn("dbo.TM_Trainee_Eva", "incharge_comments", c => c.String(maxLength: 1500));
            DropColumn("dbo.TM_Trainee_Eva", "trainee_developmental");
            DropColumn("dbo.TM_Trainee_Eva", "trainee_done_well");
            DropColumn("dbo.TM_Trainee_Eva", "trainee_learned");
        }
    }
}
