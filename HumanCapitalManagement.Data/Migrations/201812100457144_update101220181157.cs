namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update101220181157 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TM_Divisions", "approve_description", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_Divisions", "approve_description", c => c.String(maxLength: 50));
        }
    }
}
