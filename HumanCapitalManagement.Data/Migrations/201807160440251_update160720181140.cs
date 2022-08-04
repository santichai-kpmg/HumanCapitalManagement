namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update160720181140 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "candidate_password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Candidates", "candidate_password");
        }
    }
}
