namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update050620191139 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PES_Nomination_KPIs", "PES_Final_Rating_Year_Id", c => c.Int());
            CreateIndex("dbo.PES_Nomination_KPIs", "PES_Final_Rating_Year_Id");
            AddForeignKey("dbo.PES_Nomination_KPIs", "PES_Final_Rating_Year_Id", "dbo.PES_Final_Rating_Year", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PES_Nomination_KPIs", "PES_Final_Rating_Year_Id", "dbo.PES_Final_Rating_Year");
            DropIndex("dbo.PES_Nomination_KPIs", new[] { "PES_Final_Rating_Year_Id" });
            DropColumn("dbo.PES_Nomination_KPIs", "PES_Final_Rating_Year_Id");
        }
    }
}
