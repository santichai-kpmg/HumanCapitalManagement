namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFKAdminID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", c => c.Int());
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa", "Id");
            DropColumn("dbo.TM_Department_Visa", "AdminID");
            DropColumn("dbo.TM_Position_Visa", "AdminID");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "AdminID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_EmployeeForeign_Visa", "AdminID", c => c.String(maxLength: 50));
            AddColumn("dbo.TM_Position_Visa", "AdminID", c => c.String(maxLength: 50));
            AddColumn("dbo.TM_Department_Visa", "AdminID", c => c.String(maxLength: 50));
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa");
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Admin_Visa_Id" });
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
        }
    }
}
