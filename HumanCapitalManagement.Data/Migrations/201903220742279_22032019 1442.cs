namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _220320191442 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeSheet_Detail", "TimeSheet_Form_Id", "dbo.TimeSheet_Form");
            DropIndex("dbo.TimeSheet_Detail", new[] { "TimeSheet_Form_Id" });
            AddColumn("dbo.TimeSheet_Detail", "TimeSheet_Form_Id1", c => c.Int());
            CreateIndex("dbo.TimeSheet_Detail", "TimeSheet_Form_Id1");
            AddForeignKey("dbo.TimeSheet_Detail", "TimeSheet_Form_Id1", "dbo.TimeSheet_Form", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSheet_Detail", "TimeSheet_Form_Id1", "dbo.TimeSheet_Form");
            DropIndex("dbo.TimeSheet_Detail", new[] { "TimeSheet_Form_Id1" });
            DropColumn("dbo.TimeSheet_Detail", "TimeSheet_Form_Id1");
            CreateIndex("dbo.TimeSheet_Detail", "TimeSheet_Form_Id");
            AddForeignKey("dbo.TimeSheet_Detail", "TimeSheet_Form_Id", "dbo.TimeSheet_Form", "Id");
        }
    }
}
