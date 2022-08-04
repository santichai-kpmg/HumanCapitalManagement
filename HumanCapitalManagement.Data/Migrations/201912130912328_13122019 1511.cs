namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _131220191511 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MiniHeart_Detail", "Group", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MiniHeart_Detail", "Group");
        }
    }
}
