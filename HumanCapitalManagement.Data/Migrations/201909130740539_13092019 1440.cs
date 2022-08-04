namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _130920191440 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Probation_Group_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active_Status = c.String(maxLength: 10),
                        action_date = c.DateTime(),
                        description = c.String(maxLength: 500),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TM_Probation_Question", "TM_Probation_Group_Question_Id", c => c.Int());
            CreateIndex("dbo.TM_Probation_Question", "TM_Probation_Group_Question_Id");
            AddForeignKey("dbo.TM_Probation_Question", "TM_Probation_Group_Question_Id", "dbo.TM_Probation_Group_Question", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Probation_Question", "TM_Probation_Group_Question_Id", "dbo.TM_Probation_Group_Question");
            DropIndex("dbo.TM_Probation_Question", new[] { "TM_Probation_Group_Question_Id" });
            DropColumn("dbo.TM_Probation_Question", "TM_Probation_Group_Question_Id");
            DropTable("dbo.TM_Probation_Group_Question");
        }
    }
}
