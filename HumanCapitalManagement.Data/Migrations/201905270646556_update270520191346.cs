namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update270520191346 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PES_Nomination_KPIs", "PES_Nomination_Id", "dbo.PES_Nomination");
            DropIndex("dbo.PES_Nomination_KPIs", new[] { "PES_Nomination_Id" });
            AddColumn("dbo.PES_Nomination_KPIs", "user_id", c => c.String(maxLength: 50));
            DropColumn("dbo.PES_Nomination_KPIs", "how_to");
            DropColumn("dbo.PES_Nomination_KPIs", "final_remark");
            DropColumn("dbo.PES_Nomination_KPIs", "PES_Nomination_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PES_Nomination_KPIs", "PES_Nomination_Id", c => c.Int());
            AddColumn("dbo.PES_Nomination_KPIs", "final_remark", c => c.String());
            AddColumn("dbo.PES_Nomination_KPIs", "how_to", c => c.String());
            DropColumn("dbo.PES_Nomination_KPIs", "user_id");
            CreateIndex("dbo.PES_Nomination_KPIs", "PES_Nomination_Id");
            AddForeignKey("dbo.PES_Nomination_KPIs", "PES_Nomination_Id", "dbo.PES_Nomination", "Id");
        }
    }
}
