namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update200920181438 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PTR_Evaluation_KPIs", "group_target", c => c.String(maxLength: 1000));
            AddColumn("dbo.PTR_Evaluation_KPIs", "group_target_max", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PTR_Evaluation_KPIs", "group_target_max");
            DropColumn("dbo.PTR_Evaluation_KPIs", "group_target");
        }
    }
}
