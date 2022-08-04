namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _250120211634addcolumnforshownme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MiniHeart_Detail2021", "Show_Name", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MiniHeart_Detail2021", "Show_Name");
        }
    }
}
