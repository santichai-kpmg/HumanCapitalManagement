namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _200920191308 : DbMigration
    {
        public override void Up()
        {
            // DropColumn("dbo.Probation_Details", "TM_Probation_Question_Id");
            //RenameColumn(table: "dbo.Probation_Details", name: "TM_Probation_Question_Id1", newName: "TM_Probation_Question_Id");
            //  RenameIndex(table: "dbo.Probation_Details", name: "IX_TM_Probation_Question_Id1", newName: "IX_TM_Probation_Question_Id");
        }

        public override void Down()
        {
            // RenameIndex(table: "dbo.Probation_Details", name: "IX_TM_Probation_Question_Id", newName: "IX_TM_Probation_Question_Id1");
            //  RenameColumn(table: "dbo.Probation_Details", name: "TM_Probation_Question_Id", newName: "TM_Probation_Question_Id1");
            //  AddColumn("dbo.Probation_Details", "TM_Probation_Question_Id", c => c.Int());
        }
    }
}
