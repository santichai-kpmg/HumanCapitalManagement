namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update270720101706 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Universitys", "type", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Universitys", "type");
        }
    }
}
