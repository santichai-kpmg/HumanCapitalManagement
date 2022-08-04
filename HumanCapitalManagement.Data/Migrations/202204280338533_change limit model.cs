namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changelimitmodel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Consent_Asnwer", "Create_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.Consent_Asnwer", "Update_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.Consent_Main_Form", "Create_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.Consent_Main_Form", "Update_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.Consent_Asnwer_Log", "Create_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.Consent_Asnwer_Log", "Update_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.Consent_Spacial", "Create_User", c => c.String(maxLength: 50));
            AlterColumn("dbo.Consent_Spacial", "Update_User", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Consent_Spacial", "Update_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Consent_Spacial", "Create_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Consent_Asnwer_Log", "Update_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Consent_Asnwer_Log", "Create_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Consent_Main_Form", "Update_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Consent_Main_Form", "Create_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Consent_Asnwer", "Update_User", c => c.String(maxLength: 10));
            AlterColumn("dbo.Consent_Asnwer", "Create_User", c => c.String(maxLength: 10));
        }
    }
}
