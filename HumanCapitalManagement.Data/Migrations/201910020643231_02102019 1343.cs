namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _021020191343 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Form", "Extend_Form", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Form", "Extend_Form");
        }
    }
}
