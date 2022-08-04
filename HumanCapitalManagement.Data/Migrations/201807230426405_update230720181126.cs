namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update230720181126 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "candidate_prefix_TH", c => c.String());
            AddColumn("dbo.TM_Candidates", "candidate_FNameTH", c => c.String());
            AddColumn("dbo.TM_Candidates", "candiate_LNameTH", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Candidates", "candiate_LNameTH");
            DropColumn("dbo.TM_Candidates", "candidate_FNameTH");
            DropColumn("dbo.TM_Candidates", "candidate_prefix_TH");
        }
    }
}
