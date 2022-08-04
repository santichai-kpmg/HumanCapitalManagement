namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update07062019150 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PES_Nomination_Default_Committee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_no = c.String(maxLength: 50),
                        committee_type = c.String(maxLength: 10),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        PES_Nomination_Year_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PES_Nomination_Year", t => t.PES_Nomination_Year_Id)
                .Index(t => t.PES_Nomination_Year_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PES_Nomination_Default_Committee", "PES_Nomination_Year_Id", "dbo.PES_Nomination_Year");
            DropIndex("dbo.PES_Nomination_Default_Committee", new[] { "PES_Nomination_Year_Id" });
            DropTable("dbo.PES_Nomination_Default_Committee");
        }
    }
}
