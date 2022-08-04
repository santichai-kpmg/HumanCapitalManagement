namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update040220191540 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TM_Candidates", "trainee_email", c => c.String(maxLength: 250));
            //AddColumn("dbo.TM_Candidates", "is_verify", c => c.String(maxLength: 10));
            //AddColumn("dbo.TM_Candidates", "verify_code", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TM_Candidates", "verify_code");
            //DropColumn("dbo.TM_Candidates", "is_verify");
            //DropColumn("dbo.TM_Candidates", "trainee_email");
        }
    }
}
