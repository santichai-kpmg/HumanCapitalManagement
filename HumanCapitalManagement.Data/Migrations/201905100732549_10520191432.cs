namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10520191432 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet_Detail", "Source_Type", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet_Detail", "Source_Type");
        }
    }
}
