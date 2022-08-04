namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _040220201633addprobationwithout : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Probation_With_Out",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Staff_No = c.String(maxLength: 10),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.String(maxLength: 10),
                        Update_Date = c.DateTime(),
                        Update_User = c.String(maxLength: 10),
                        Remark = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Probation_With_Out");
        }
    }
}
