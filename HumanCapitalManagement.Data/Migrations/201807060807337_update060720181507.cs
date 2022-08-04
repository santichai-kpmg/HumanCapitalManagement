namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update060720181507 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PTR_Evaluation_File", "description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PTR_Evaluation_File", "description");
        }
    }
}
