namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240120192124 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogUpdateEmpNO",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        action_date = c.DateTime(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogUpdateEmpNO");
        }
    }
}
