namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update280520191606 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Perdiem_Transport", "TM_Time_Type_Id", "dbo.TM_Time_Type");
            DropIndex("dbo.Perdiem_Transport", new[] { "TM_Time_Type_Id" });
          
            DropColumn("dbo.Perdiem_Transport", "TM_Time_Type_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Perdiem_Transport", "TM_Time_Type_Id", c => c.Int());
           
            CreateIndex("dbo.Perdiem_Transport", "TM_Time_Type_Id");
            AddForeignKey("dbo.Perdiem_Transport", "TM_Time_Type_Id", "dbo.TM_Time_Type", "Id");
        }
    }
}
