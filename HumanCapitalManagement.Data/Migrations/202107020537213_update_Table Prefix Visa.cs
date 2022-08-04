namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_TablePrefixVisa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Prefix_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrefixId = c.String(maxLength: 50),
                        PrefixNameTH = c.String(maxLength: 250),
                        PrefixNameEN = c.String(maxLength: 250),
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
            DropTable("dbo.TM_Prefix_Visa");
        }
    }
}
