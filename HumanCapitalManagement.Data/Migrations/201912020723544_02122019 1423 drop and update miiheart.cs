namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _021220191423dropandupdatemiiheart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MiniHeart_Detail", "Emp_No", c => c.String(maxLength: 10));
            DropColumn("dbo.MiniHeart_Detail", "Emp_Id");
            DropColumn("dbo.MiniHeart_Main", "Start_Peroid");
            DropColumn("dbo.MiniHeart_Main", "End_Peroid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MiniHeart_Main", "End_Peroid", c => c.DateTime());
            AddColumn("dbo.MiniHeart_Main", "Start_Peroid", c => c.DateTime());
            AddColumn("dbo.MiniHeart_Detail", "Emp_Id", c => c.String(maxLength: 2));
            DropColumn("dbo.MiniHeart_Detail", "Emp_No");
        }
    }
}
