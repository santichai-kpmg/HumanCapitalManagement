namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update230820181403 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Candidates", "prefixEN_Id", c => c.Int());
            AddColumn("dbo.TM_Candidates", "prefixTH_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidates", "prefixEN_Id");
            CreateIndex("dbo.TM_Candidates", "prefixTH_Id");
            AddForeignKey("dbo.TM_Candidates", "prefixEN_Id", "dbo.TM_Prefix", "Id");
            AddForeignKey("dbo.TM_Candidates", "prefixTH_Id", "dbo.TM_Prefix", "Id");
            DropColumn("dbo.TM_Candidates", "candidate_prefix_TH");
            DropColumn("dbo.TM_Candidates", "prefix_th");
            DropColumn("dbo.TM_Candidates", "candidate_Prefix");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_Candidates", "candidate_Prefix", c => c.String());
            AddColumn("dbo.TM_Candidates", "prefix_th", c => c.String(maxLength: 10));
            AddColumn("dbo.TM_Candidates", "candidate_prefix_TH", c => c.String());
            DropForeignKey("dbo.TM_Candidates", "prefixTH_Id", "dbo.TM_Prefix");
            DropForeignKey("dbo.TM_Candidates", "prefixEN_Id", "dbo.TM_Prefix");
            DropIndex("dbo.TM_Candidates", new[] { "prefixTH_Id" });
            DropIndex("dbo.TM_Candidates", new[] { "prefixEN_Id" });
            DropColumn("dbo.TM_Candidates", "prefixTH_Id");
            DropColumn("dbo.TM_Candidates", "prefixEN_Id");
        }
    }
}
