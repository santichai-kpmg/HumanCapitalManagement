namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updtetableemployeeothernatin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TM_Document_Employee", "TM_Employee_Id", "dbo.TM_Employee");
            DropIndex("dbo.TM_Document_Employee", new[] { "TM_Employee_Id" });
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Department_Visa", t => t.TM_Department_Visa_Id)
                .ForeignKey("dbo.TM_Position_Visa", t => t.TM_Position_Visa_Id)
                .Index(t => t.TM_Department_Visa_Id)
                .Index(t => t.TM_Position_Visa_Id);
            
            CreateTable(
                "dbo.TM_Department_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Departmentname = c.String(maxLength: 500),
                        Floor = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 50),
                        AdminID = c.String(maxLength: 50),
                        create_user = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        update_date = c.DateTime(),
                        update_user = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_Position_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionName = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 50),
                        AdminID = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_EmployeeForeign_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeNo = c.String(maxLength: 500),
                        TM_Prefix_Id = c.Int(),
                        Employeename = c.String(maxLength: 500),
                        Employeesurname = c.String(maxLength: 500),
                        TM_Department_Visa_Id = c.Int(),
                        TM_Position_Visa_Id = c.Int(),
                        location = c.String(maxLength: 500),
                        ext = c.String(maxLength: 500),
                        email = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 50),
                        AdminID = c.String(maxLength: 50),
                        create_user = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        update_date = c.DateTime(),
                        update_user = c.String(),
                        family_group = c.String(maxLength: 50),
                        remark = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Department_Visa", t => t.TM_Department_Visa_Id)
                .ForeignKey("dbo.TM_Position_Visa", t => t.TM_Position_Visa_Id)
                .ForeignKey("dbo.TM_Prefix_Visa", t => t.TM_Prefix_Id)
                .Index(t => t.TM_Prefix_Id)
                .Index(t => t.TM_Department_Visa_Id)
                .Index(t => t.TM_Position_Visa_Id);
            
            AddColumn("dbo.TM_Document_Employee", "TM_EmployeeForeign_Visa_Id", c => c.Int());
            CreateIndex("dbo.TM_Document_Employee", "TM_EmployeeForeign_Visa_Id");
            AddForeignKey("dbo.TM_Document_Employee", "TM_EmployeeForeign_Visa_Id", "dbo.TM_EmployeeForeign_Visa", "Id");
            DropColumn("dbo.TM_Document_Employee", "TM_Employee_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_Document_Employee", "TM_Employee_Id", c => c.Int());
            DropForeignKey("dbo.TM_Document_Employee", "TM_EmployeeForeign_Visa_Id", "dbo.TM_EmployeeForeign_Visa");
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Prefix_Id", "dbo.TM_Prefix_Visa");
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Position_Visa_Id", "dbo.TM_Position_Visa");
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Department_Visa_Id", "dbo.TM_Department_Visa");
            DropForeignKey("dbo.TM_Admin_Visa", "TM_Position_Visa_Id", "dbo.TM_Position_Visa");
            DropForeignKey("dbo.TM_Admin_Visa", "TM_Department_Visa_Id", "dbo.TM_Department_Visa");
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Position_Visa_Id" });
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Department_Visa_Id" });
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Prefix_Id" });
            DropIndex("dbo.TM_Document_Employee", new[] { "TM_EmployeeForeign_Visa_Id" });
            DropIndex("dbo.TM_Admin_Visa", new[] { "TM_Position_Visa_Id" });
            DropIndex("dbo.TM_Admin_Visa", new[] { "TM_Department_Visa_Id" });
            DropColumn("dbo.TM_Document_Employee", "TM_EmployeeForeign_Visa_Id");
            DropTable("dbo.TM_EmployeeForeign_Visa");
            DropTable("dbo.TM_Position_Visa");
            DropTable("dbo.TM_Department_Visa");
            DropTable("dbo.TM_Admin_Visa");
            CreateIndex("dbo.TM_Document_Employee", "TM_Employee_Id");
            AddForeignKey("dbo.TM_Document_Employee", "TM_Employee_Id", "dbo.TM_Employee", "Id");
        }
    }
}
