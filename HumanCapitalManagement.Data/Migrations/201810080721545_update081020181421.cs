namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update081020181421 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TBmst_PreviousFY");
            AlterColumn("dbo.TBmst_PreviousFY", "nYear", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TBmst_PreviousFY", new[] { "EmployeeNo", "nYear" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TBmst_PreviousFY");
            AlterColumn("dbo.TBmst_PreviousFY", "nYear", c => c.Int());
            AddPrimaryKey("dbo.TBmst_PreviousFY", "EmployeeNo");
        }
    }
}
