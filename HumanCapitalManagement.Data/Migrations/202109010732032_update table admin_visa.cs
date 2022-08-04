namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableadmin_visa : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa", "Id");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employee_Staff");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employee_Staff", c => c.String(maxLength: 500));
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa");
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Admin_Visa_Id" });
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
            DropTable("dbo.TM_Admin_Visa");
        }
    }
}
