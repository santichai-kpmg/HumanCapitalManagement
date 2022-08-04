namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update160720180934 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PTR_Evaluation", "comments", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PTR_Evaluation", "comments", c => c.String(maxLength: 1500));
        }
    }
}
