namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update160620192220 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MailContents", "nCId", c => c.Int());
            AddColumn("dbo.PTR_Evaluation_KPIs", "sbu", c => c.String(maxLength: 1000));
            AddColumn("dbo.PTR_Evaluation_KPIs", "su", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PTR_Evaluation_KPIs", "su");
            DropColumn("dbo.PTR_Evaluation_KPIs", "sbu");
            DropColumn("dbo.MailContents", "nCId");
        }
    }
}
