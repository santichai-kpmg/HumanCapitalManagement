namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240620191030 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PES_Nomination_Signatures", "TM_Annual_Rating_Id", c => c.Int());
            CreateIndex("dbo.PES_Nomination_Signatures", "TM_Annual_Rating_Id");
            AddForeignKey("dbo.PES_Nomination_Signatures", "TM_Annual_Rating_Id", "dbo.TM_Annual_Rating", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PES_Nomination_Signatures", "TM_Annual_Rating_Id", "dbo.TM_Annual_Rating");
            DropIndex("dbo.PES_Nomination_Signatures", new[] { "TM_Annual_Rating_Id" });
            DropColumn("dbo.PES_Nomination_Signatures", "TM_Annual_Rating_Id");
        }
    }
}
