namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240720181812 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        id_card = c.String(maxLength: 20),
                        first_name_en = c.String(maxLength: 250),
                        last_name_en = c.String(maxLength: 250),
                        candidate_NickName = c.String(),
                        candidate_prefix_TH = c.String(),
                        candidate_FNameTH = c.String(),
                        candiate_LNameTH = c.String(),
                        candidate_phone = c.String(maxLength: 50),
                        candidate_Email = c.String(),
                        candidate_ProfessionalQualification = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempTransactions");
        }
    }
}
