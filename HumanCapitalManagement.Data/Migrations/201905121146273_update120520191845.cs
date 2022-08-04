namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update120520191845 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PTR_Eva_Incidents", "nscore", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_PTR_Eva_Incidents", "nscore");
        }
    }
}
