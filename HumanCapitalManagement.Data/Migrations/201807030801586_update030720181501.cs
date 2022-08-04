namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update030720181501 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PTR_Evaluation_Year", "sfile64", c => c.Binary());
            AddColumn("dbo.PTR_Evaluation_Year", "sfileType", c => c.String(maxLength: 20));
            AddColumn("dbo.PTR_Evaluation_Year", "sfile_oldname", c => c.String(maxLength: 250));
            AddColumn("dbo.PTR_Evaluation_Year", "sfile_newname", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PTR_Evaluation_Year", "sfile_newname");
            DropColumn("dbo.PTR_Evaluation_Year", "sfile_oldname");
            DropColumn("dbo.PTR_Evaluation_Year", "sfileType");
            DropColumn("dbo.PTR_Evaluation_Year", "sfile64");
        }
    }
}
