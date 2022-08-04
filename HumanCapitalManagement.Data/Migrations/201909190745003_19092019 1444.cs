namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _190920191444 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Probation_Group_Question", "Probation_Form_Id", c => c.Int());
            CreateIndex("dbo.TM_Probation_Group_Question", "Probation_Form_Id");
            AddForeignKey("dbo.TM_Probation_Group_Question", "Probation_Form_Id", "dbo.Probation_Form", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Probation_Group_Question", "Probation_Form_Id", "dbo.Probation_Form");
            DropIndex("dbo.TM_Probation_Group_Question", new[] { "Probation_Form_Id" });
            DropColumn("dbo.TM_Probation_Group_Question", "Probation_Form_Id");
        }
    }
}
