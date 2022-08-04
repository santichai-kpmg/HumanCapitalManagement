namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _130920191605 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Probation_Group_Question", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Probation_Group_Question", "Name");
        }
    }
}
