namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefixfktablequestionmass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question");
            DropIndex("dbo.TM_PIntern_Form_Question", new[] { "TM_PIntern_Mass_Form_Question_Id" });
            DropColumn("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id", c => c.Int());
            CreateIndex("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id");
            AddForeignKey("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question", "Id");
        }
    }
}
