namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _230720191614 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Positive = c.String(maxLength: 1000),
                        Strength = c.String(maxLength: 1000),
                        Need_Improvement = c.String(maxLength: 1000),
                        Recommendations = c.String(maxLength: 1000),
                        Rate = c.Int(),
                        Type = c.String(maxLength: 1),
                        Active_Status = c.String(maxLength: 10),
                        Create_Date = c.DateTime(),
                        Create_User = c.Int(),
                        Update_Date = c.DateTime(),
                        Update_User = c.Int(),
                        Status = c.String(maxLength: 10),
                        Given_User = c.String(maxLength: 50),
                        Request_User = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Feedbacks");
        }
    }
}
