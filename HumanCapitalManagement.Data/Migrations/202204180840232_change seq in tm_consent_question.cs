namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeseqintm_consent_question : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TM_Consent_Question", "Seq", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_Consent_Question", "Seq", c => c.Int(nullable: false));
        }
    }
}
