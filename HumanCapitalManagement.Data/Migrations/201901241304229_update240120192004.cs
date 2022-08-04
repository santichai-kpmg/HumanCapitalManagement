namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update240120192004 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TM_Candidate_EmpNO",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        emp_no = c.String(maxLength: 250),
                        active_status = c.String(maxLength: 10),
                        create_date = c.DateTime(),
                        create_user = c.String(maxLength: 50),
                        update_date = c.DateTime(),
                        update_user = c.String(maxLength: 50),
                        seq = c.Int(),
                        TM_Candidates_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TM_Candidates", t => t.TM_Candidates_Id)
                .Index(t => t.TM_Candidates_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TM_Candidate_EmpNO", "TM_Candidates_Id", "dbo.TM_Candidates");
            DropIndex("dbo.TM_Candidate_EmpNO", new[] { "TM_Candidates_Id" });
            DropTable("dbo.TM_Candidate_EmpNO");
        }
    }
}
