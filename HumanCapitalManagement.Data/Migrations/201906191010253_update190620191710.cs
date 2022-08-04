namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update190620191710 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PES_Nomination_Signatures", "Agree_Status", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PES_Nomination_Signatures", "Agree_Status");
        }
    }
}
