namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableEMPV1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TM_Admin_Visa", "TM_Department_Visa_Id", "dbo.TM_Department_Visa");
            DropForeignKey("dbo.TM_Admin_Visa", "TM_Position_Visa_Id", "dbo.TM_Position_Visa");
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa");
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Department_Visa_Id", "dbo.TM_Department_Visa");
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Position_Visa_Id", "dbo.TM_Position_Visa");
            DropIndex("dbo.TM_Admin_Visa", new[] { "TM_Department_Visa_Id" });
            DropIndex("dbo.TM_Admin_Visa", new[] { "TM_Position_Visa_Id" });
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Department_Visa_Id" });
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Position_Visa_Id" });
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Admin_Visa_Id" });
            CreateTable(
                "dbo.TM_Company_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        company_name = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_EmployeeForeign_Visa", "TM_Company_Id", c => c.Int());
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employeename_ENG", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employeesurname_ENG", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employee_Middle_ENG", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employeename_TH", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employeesurname_TH", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employee_telephone", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employee_Staff", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employee_email", c => c.String(maxLength: 500));
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Company_Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Company_Id", "dbo.TM_Company_Visa", "Id");
            DropColumn("dbo.TM_Document_Employee", "seq");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employeename");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employeesurname");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_middle");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_name_th");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_lastname_th");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Department_Visa_Id");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Position_Visa_Id");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_telephone");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "location");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "ext");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "email");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
            DropTable("dbo.TM_Admin_Visa");
            DropTable("dbo.TM_Department_Visa");
            DropTable("dbo.TM_Position_Visa");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TM_Position_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionName = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Department_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Departmentname = c.String(maxLength: 500),
                        Floor = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 50),
                        create_user = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        update_date = c.DateTime(),
                        update_user = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Admin_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdminName = c.String(maxLength: 500),
                        Lastname = c.String(maxLength: 500),
                        Username = c.String(maxLength: 500),
                        TxtPassword = c.String(maxLength: 500),
                        TM_Department_Visa_Id = c.Int(),
                        TM_Position_Visa_Id = c.Int(),
                        Prioritytype = c.String(maxLength: 500),
                        Datejoin = c.DateTime(),
                        active_status = c.String(maxLength: 50),
                        Recordby = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", c => c.Int());
            AddColumn("dbo.TM_EmployeeForeign_Visa", "email", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "ext", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "location", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_telephone", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "TM_Position_Visa_Id", c => c.Int());
            AddColumn("dbo.TM_EmployeeForeign_Visa", "TM_Department_Visa_Id", c => c.Int());
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_lastname_th", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_name_th", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_middle", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employeesurname", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "Employeename", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_Document_Employee", "seq", c => c.String(maxLength: 10));
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Company_Id", "dbo.TM_Company_Visa");
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Company_Id" });
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employee_email");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employee_Staff");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employee_telephone");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employeesurname_TH");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employeename_TH");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employee_Middle_ENG");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employeesurname_ENG");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "Employeename_ENG");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Company_Id");
            DropTable("dbo.TM_Company_Visa");
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id");
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Position_Visa_Id");
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Department_Visa_Id");
            CreateIndex("dbo.TM_Admin_Visa", "TM_Position_Visa_Id");
            CreateIndex("dbo.TM_Admin_Visa", "TM_Department_Visa_Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Position_Visa_Id", "dbo.TM_Position_Visa", "Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Department_Visa_Id", "dbo.TM_Department_Visa", "Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Admin_Visa_Id", "dbo.TM_Admin_Visa", "Id");
            AddForeignKey("dbo.TM_Admin_Visa", "TM_Position_Visa_Id", "dbo.TM_Position_Visa", "Id");
            AddForeignKey("dbo.TM_Admin_Visa", "TM_Department_Visa_Id", "dbo.TM_Department_Visa", "Id");
        }
    }
}
