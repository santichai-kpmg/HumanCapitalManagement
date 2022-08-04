namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _230720191810 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feedbacks", "Create_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Feedbacks", "Update_User", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feedbacks", "Update_User", c => c.Int());
            AlterColumn("dbo.Feedbacks", "Create_User", c => c.Int());
        }
    }
}
