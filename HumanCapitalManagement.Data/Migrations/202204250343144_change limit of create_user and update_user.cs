namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changelimitofcreate_userandupdate_user : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TM_Consent_Form", "Create_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.TM_Consent_Form", "Update_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.TM_Consent_Question", "Create_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.TM_Consent_Question", "Update_User", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TM_Consent_Question", "Update_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.TM_Consent_Question", "Create_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.TM_Consent_Form", "Update_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.TM_Consent_Form", "Create_User", c => c.String(maxLength: 10));
        }
    }
}
