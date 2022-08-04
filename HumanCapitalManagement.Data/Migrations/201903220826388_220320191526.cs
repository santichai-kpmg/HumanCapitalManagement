namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _220320191526 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Time_Type", "colors", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Time_Type", "colors");
        }
    }
}
