namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _230920191129 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Form", "HR_No", c => c.String(maxLength: 10));
            AddColumn("dbo.Probation_Form", "HR_Submit_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Form", "HR_Submit_Date");
            DropColumn("dbo.Probation_Form", "HR_No");
        }
    }
}
