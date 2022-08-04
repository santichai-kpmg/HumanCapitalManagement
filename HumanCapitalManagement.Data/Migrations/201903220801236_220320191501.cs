namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _220320191501 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TimeSheet_Detail", "TimeSheet_Form_Id");
            RenameColumn(table: "dbo.TimeSheet_Detail", name: "TimeSheet_Form_Id1", newName: "TimeSheet_Form_Id");
            RenameIndex(table: "dbo.TimeSheet_Detail", name: "IX_TimeSheet_Form_Id1", newName: "IX_TimeSheet_Form_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TimeSheet_Detail", name: "IX_TimeSheet_Form_Id", newName: "IX_TimeSheet_Form_Id1");
            RenameColumn(table: "dbo.TimeSheet_Detail", name: "TimeSheet_Form_Id", newName: "TimeSheet_Form_Id1");
            AddColumn("dbo.TimeSheet_Detail", "TimeSheet_Form_Id", c => c.Int());
        }
    }
}
