namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changelimitnamemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Consent_Main_Form", "Employee_no", c => c.String(maxLength: 1000));
            DropColumn("dbo.Consent_Main_Form", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Consent_Main_Form", "Name", c => c.String(maxLength: 1000));
            DropColumn("dbo.Consent_Main_Form", "Employee_no");
        }
    }
}
