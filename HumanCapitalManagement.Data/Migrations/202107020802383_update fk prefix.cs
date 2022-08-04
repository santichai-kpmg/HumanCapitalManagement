namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefkprefix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TM_Employee", "TM_Prefix_Id", "dbo.TM_Prefix");
            AddForeignKey("dbo.TM_Employee", "TM_Prefix_Id", "dbo.TM_Prefix_Visa", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Employee", "TM_Prefix_Id", "dbo.TM_Prefix_Visa");
            AddForeignKey("dbo.TM_Employee", "TM_Prefix_Id", "dbo.TM_Prefix", "Id");
        }
    }
}
