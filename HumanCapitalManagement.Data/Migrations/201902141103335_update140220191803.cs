namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update140220191803 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_WorkExperience", "create_date", c => c.DateTime());
            AddColumn("dbo.TM_WorkExperience", "create_user", c => c.String(maxLength: 50));
            AddColumn("dbo.TM_WorkExperience", "update_date", c => c.DateTime());
            AddColumn("dbo.TM_WorkExperience", "update_user", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_WorkExperience", "update_user");
            DropColumn("dbo.TM_WorkExperience", "update_date");
            DropColumn("dbo.TM_WorkExperience", "create_user");
            DropColumn("dbo.TM_WorkExperience", "create_date");
        }
    }
}
