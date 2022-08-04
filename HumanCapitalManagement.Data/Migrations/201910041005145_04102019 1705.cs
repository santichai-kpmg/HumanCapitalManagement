namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _041020191705 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Action_Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seq = c.Int(),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 50),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 50),
                        sfile64 = c.Binary(),
                        sfileType = c.String(maxLength: 20),
                        sfile_oldname = c.String(maxLength: 250),
                        sfile_newname = c.String(maxLength: 250),
                        Description = c.String(maxLength: 500),
                        Probation_Form_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Probation_Form", t => t.Probation_Form_Id)
                .Index(t => t.Probation_Form_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Action_Plans", "Probation_Form_Id", "dbo.Probation_Form");
            DropIndex("dbo.Action_Plans", new[] { "Probation_Form_Id" });
            DropTable("dbo.Action_Plans");
        }
    }
}
