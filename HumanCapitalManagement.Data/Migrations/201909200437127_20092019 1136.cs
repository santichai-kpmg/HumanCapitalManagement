namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _200920191136 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Probation_Detail", newName: "Probation_Details");
            DropColumn("dbo.Probation_Details", "add");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Probation_Details", "add", c => c.String(maxLength: 1000));
            RenameTable(name: "dbo.Probation_Details", newName: "Probation_Detail");
        }
    }
}
