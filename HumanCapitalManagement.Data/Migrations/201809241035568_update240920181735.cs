namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240920181735 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_FY_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        para = c.Decimal(precision: 18, scale: 2),
                        aa = c.Decimal(precision: 18, scale: 2),
                        sr = c.Decimal(precision: 18, scale: 2),
                        am = c.Decimal(precision: 18, scale: 2),
                        mgr = c.Decimal(precision: 18, scale: 2),
                        ad = c.Decimal(precision: 18, scale: 2),
                        dir = c.Decimal(precision: 18, scale: 2),
                        ptr = c.Decimal(precision: 18, scale: 2),
                        TM_Divisions_Id = c.Int(),
                        TM_FY_Plan_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Divisions", t => t.TM_Divisions_Id)
                .ForeignKey("dbo.TM_FY_Plan", t => t.TM_FY_Plan_Id)
                .Index(t => t.TM_Divisions_Id)
                .Index(t => t.TM_FY_Plan_Id);
            
            CreateTable(
                "dbo.TM_FY_Plan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fy_year = c.DateTime(),
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
            DropForeignKey("dbo.TM_FY_Detail", "TM_FY_Plan_Id", "dbo.TM_FY_Plan");
            DropForeignKey("dbo.TM_FY_Detail", "TM_Divisions_Id", "dbo.TM_Divisions");
            DropIndex("dbo.TM_FY_Detail", new[] { "TM_FY_Plan_Id" });
            DropIndex("dbo.TM_FY_Detail", new[] { "TM_Divisions_Id" });
            DropTable("dbo.TM_FY_Plan");
            DropTable("dbo.TM_FY_Detail");
        }
    }
}
