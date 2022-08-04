namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update200820181102 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Additional_Questions", "is_validate", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Additional_Questions", "is_validate");
        }
    }
}
