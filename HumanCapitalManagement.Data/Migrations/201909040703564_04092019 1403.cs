namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _040920191403 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Company_Trainee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitGroupID = c.String(maxLength: 50),
                        UnitGroupName = c.String(maxLength: 50),
                        Company = c.String(maxLength: 1000),
                        Company_Short = c.String(maxLength: 50),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TM_Company_Trainee");
        }
    }
}
