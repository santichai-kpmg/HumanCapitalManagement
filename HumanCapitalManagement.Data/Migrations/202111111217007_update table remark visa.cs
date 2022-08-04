namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableremarkvisa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Remark_Visa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RemarkEN = c.String(maxLength: 1000),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_EmployeeForeign_Visa", "TM_Remark_Id", c => c.Int());
            CreateIndex("dbo.TM_EmployeeForeign_Visa", "TM_Remark_Id");
            AddForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Remark_Id", "dbo.TM_Remark_Visa", "Id");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "remark");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_EmployeeForeign_Visa", "remark", c => c.String(maxLength: 500));
            DropForeignKey("dbo.TM_EmployeeForeign_Visa", "TM_Remark_Id", "dbo.TM_Remark_Visa");
            DropIndex("dbo.TM_EmployeeForeign_Visa", new[] { "TM_Remark_Id" });
            DropColumn("dbo.TM_EmployeeForeign_Visa", "TM_Remark_Id");
            DropTable("dbo.TM_Remark_Visa");
        }
    }
}
