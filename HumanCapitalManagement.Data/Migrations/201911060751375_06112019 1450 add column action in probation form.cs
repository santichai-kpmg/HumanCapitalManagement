namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _061120191450addcolumnactioninprobationform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Form", "Staff_Action", c => c.String(maxLength: 10));
            AddColumn("dbo.Probation_Form", "PM_Action", c => c.String(maxLength: 10));
            AddColumn("dbo.Probation_Form", "GroupHead_Action", c => c.String(maxLength: 10));
            AddColumn("dbo.Probation_Form", "HOP_Action", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Form", "HOP_Action");
            DropColumn("dbo.Probation_Form", "GroupHead_Action");
            DropColumn("dbo.Probation_Form", "PM_Action");
            DropColumn("dbo.Probation_Form", "Staff_Action");
        }
    }
}
