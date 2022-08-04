namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcoltopicQu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_PIntern_Form_Question", "topic", c => c.String(maxLength: 1000));
            AlterColumn("dbo.TM_PIntern_Form_Question", "question", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_PIntern_Form_Question", "question", c => c.String(maxLength: 1000));
            DropColumn("dbo.TM_PIntern_Form_Question", "topic");
        }
    }
}
