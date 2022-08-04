namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNewTableForMassPreIntern : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_PIntern_Mass_Form_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        active_status = c.String(maxLength: 10),
                        action_date = c.DateTime(),
                        description = c.String(maxLength: 500),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TM_PIntern_Mass_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        seq = c.Int(),
                        header = c.String(maxLength: 300),
                        topic = c.String(maxLength: 1000),
                        question = c.String(),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        TM_PIntern_Mass_Form_Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_PIntern_Mass_Form_Question", t => t.TM_PIntern_Mass_Form_Question_Id)
                .Index(t => t.TM_PIntern_Mass_Form_Question_Id);
            
            AddColumn("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id", c => c.Int());
            CreateIndex("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id");
            AddForeignKey("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_PIntern_Mass_Question", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question");
            DropForeignKey("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id", "dbo.TM_PIntern_Mass_Form_Question");
            DropIndex("dbo.TM_PIntern_Mass_Question", new[] { "TM_PIntern_Mass_Form_Question_Id" });
            DropIndex("dbo.TM_PIntern_Form_Question", new[] { "TM_PIntern_Mass_Form_Question_Id" });
            DropColumn("dbo.TM_PIntern_Form_Question", "TM_PIntern_Mass_Form_Question_Id");
            DropTable("dbo.TM_PIntern_Mass_Question");
            DropTable("dbo.TM_PIntern_Mass_Form_Question");
        }
    }
}
