namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_table_staffno : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa");
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Admin_Visa_Id" });
            AddColumn("dbo.TM_EmployeeForeign_Visa", "staff_No", c => c.String(maxLength: 50));
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
            DropTable("dbo.TM_Admin_Visa");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TM_Admin_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        firstname_admin = c.String(maxLength: 500),
                        lastname_admin = c.String(maxLength: 500),
                        phone = c.String(maxLength: 500),
                        email_admin = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", c => c.Int());
            DropColumn("dbo.TM_EmployeeForeign_Visa", "staff_No");
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa", "Id");
        }
    }
}
