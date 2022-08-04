namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30072201722addfkforTM_PES_NMN_SigntureStep : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PES_NMN_SignatureStep", "PES_Nomination_Year_Id", c => c.Int());
            CreateIndex("dbo.TM_PES_NMN_SignatureStep", "PES_Nomination_Year_Id");
            AddForeignKey("dbo.TM_PES_NMN_SignatureStep", "PES_Nomination_Year_Id", "dbo.PES_Nomination_Year", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_PES_NMN_SignatureStep", "PES_Nomination_Year_Id", "dbo.PES_Nomination_Year");
            DropIndex("dbo.TM_PES_NMN_SignatureStep", new[] { "PES_Nomination_Year_Id" });
            DropColumn("dbo.TM_PES_NMN_SignatureStep", "PES_Nomination_Year_Id");
        }
    }
}
