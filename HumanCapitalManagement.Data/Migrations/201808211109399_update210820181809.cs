namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update210820181809 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Nationalities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NationalitiesId = c.String(maxLength: 50),
                        NationalitiesNameTH = c.String(maxLength: 250),
                        NationalitiesNameEN = c.String(maxLength: 250),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_Candidates", "Nationalities_Id", c => c.Int());
            CreateIndex("dbo.TM_Candidates", "Nationalities_Id");
            AddForeignKey("dbo.TM_Candidates", "Nationalities_Id", "dbo.TM_Nationalities", "Id");
            DropColumn("dbo.TM_Candidates", "candidate_Nationality");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_Candidates", "candidate_Nationality", c => c.String());
            DropForeignKey("dbo.TM_Candidates", "Nationalities_Id", "dbo.TM_Nationalities");
            DropIndex("dbo.TM_Candidates", new[] { "Nationalities_Id" });
            DropColumn("dbo.TM_Candidates", "Nationalities_Id");
            DropTable("dbo.TM_Nationalities");
        }
    }
}
