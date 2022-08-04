namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _021020191331 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Form", "Extend_Status", c => c.String(maxLength: 2));
            AddColumn("dbo.Probation_Form", "Extend_Period", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Form", "Extend_Period");
            DropColumn("dbo.Probation_Form", "Extend_Status");
        }
    }
}
