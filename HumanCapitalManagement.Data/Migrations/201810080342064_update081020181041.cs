namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update081020181041 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TBmst_PreviousFY", "nYear", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TBmst_PreviousFY", "nYear");
        }
    }
}
