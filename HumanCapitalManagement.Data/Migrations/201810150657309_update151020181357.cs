namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update151020181357 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TBmst_PreviousFY", "group_code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TBmst_PreviousFY", "group_code");
        }
    }
}
