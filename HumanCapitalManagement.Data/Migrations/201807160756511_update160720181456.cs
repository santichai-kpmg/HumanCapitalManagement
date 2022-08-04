namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update160720181456 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Trainee_Eva", "approve_type", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Trainee_Eva", "approve_type");
        }
    }
}
