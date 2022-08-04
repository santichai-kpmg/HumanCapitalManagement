namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update070820191848addidquestionpes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PTR_Eva_Questions", "nCId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_PTR_Eva_Questions", "nCId");
        }
    }
}
