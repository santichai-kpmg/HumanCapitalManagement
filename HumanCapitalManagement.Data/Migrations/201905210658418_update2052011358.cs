namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2052011358 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PTR_Evaluation_KPIs", "group_actual", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PTR_Evaluation_KPIs", "group_actual");
        }
    }
}
