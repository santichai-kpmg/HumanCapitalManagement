namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07102092016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Probation_Form", "Remark_Revise", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Probation_Form", "Remark_Revise");
        }
    }
}
