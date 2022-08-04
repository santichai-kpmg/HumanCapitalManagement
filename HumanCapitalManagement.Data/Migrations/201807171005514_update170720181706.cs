namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update170720181706 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_Trainee_Eva", "confidentiality_agreement", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_Trainee_Eva", "confidentiality_agreement");
        }
    }
}
