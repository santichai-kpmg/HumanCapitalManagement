namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update171020181556 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Company", "Country_code", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Company", "Country_code");
        }
    }
}
