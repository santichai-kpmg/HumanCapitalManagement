namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecreatetablefrvisaexpiry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Document_Employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.String(maxLength: 10),
                        doc_number = c.String(maxLength: 500),
                        date_of_issued = c.DateTime(),
                        TM_Country_Id = c.Int(),
                        valid_date = c.DateTime(),
                        active_status = c.String(maxLength: 50),
                        create_user = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        update_date = c.DateTime(),
                        update_user = c.String(),
                        TM_Employee_Id = c.Int(),
                        TM_Type_Document_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Country", t => t.TM_Country_Id)
                .ForeignKey("dbo.TM_Employee", t => t.TM_Employee_Id)
                .ForeignKey("dbo.TM_Type_Document", t => t.TM_Type_Document_Id)
                .Index(t => t.TM_Country_Id)
                .Index(t => t.TM_Employee_Id)
                .Index(t => t.TM_Type_Document_Id);
            
            CreateTable(
                "dbo.TM_Employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        em_code = c.String(maxLength: 100),
                        TM_Prefix_Id = c.Int(),
                        em_name_eng = c.String(maxLength: 500),
                        em_name_th = c.String(maxLength: 500),
                        em_middle = c.String(maxLength: 500),
                        em_lastname_eng = c.String(maxLength: 500),
                        em_lastname_th = c.String(maxLength: 500),
                        remark = c.String(maxLength: 500),
                        em_telephone = c.String(maxLength: 500),
                        em_mail = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 50),
                        create_user = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Prefix", t => t.TM_Prefix_Id)
                .Index(t => t.TM_Prefix_Id);
            
            CreateTable(
                "dbo.TM_Type_Document",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.String(maxLength: 10),
                        type_docname_eng = c.String(maxLength: 500),
                        type_docname_th = c.String(maxLength: 500),
                        active_status = c.String(maxLength: 50),
                        create_user = c.String(maxLength: 50),
                        create_date = c.DateTime(),
                        update_date = c.String(maxLength: 50),
                        update_user = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Document_Employee", "TM_Type_Document_Id", "dbo.TM_Type_Document");
            DropForeignKey("dbo.TM_Document_Employee", "TM_Employee_Id", "dbo.TM_Employee");
            DropForeignKey("dbo.TM_Employee", "TM_Prefix_Id", "dbo.TM_Prefix");
            DropForeignKey("dbo.TM_Document_Employee", "TM_Country_Id", "dbo.TM_Country");
            DropIndex("dbo.TM_Employee", new[] { "TM_Prefix_Id" });
            DropIndex("dbo.TM_Document_Employee", new[] { "TM_Type_Document_Id" });
            DropIndex("dbo.TM_Document_Employee", new[] { "TM_Employee_Id" });
            DropIndex("dbo.TM_Document_Employee", new[] { "TM_Country_Id" });
            DropTable("dbo.TM_Type_Document");
            DropTable("dbo.TM_Employee");
            DropTable("dbo.TM_Document_Employee");
        }
    }
}
