namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _017720191559updatemodel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Probation_Form", "Extend_Form", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Probation_Form", "Extend_Form", c => c.String(maxLength: 2));
        }
    }
}
