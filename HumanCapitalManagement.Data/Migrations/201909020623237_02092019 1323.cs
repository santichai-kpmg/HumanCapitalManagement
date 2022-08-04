namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _020920191323 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Perdiem_Transport", "update_date", c => c.DateTime());
            AddColumn("dbo.Perdiem_Transport", "update_user", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Perdiem_Transport", "update_user");
            DropColumn("dbo.Perdiem_Transport", "update_date");
        }
    }
}
