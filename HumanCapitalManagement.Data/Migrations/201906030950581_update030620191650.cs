namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update030620191650 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PES_Nomination", "Year_joined_KPMG", c => c.DateTime());
            AddColumn("dbo.PES_Nomination", "working_with_KPMG", c => c.Int());
            AddColumn("dbo.PES_Nomination", "working_outside_KPMG", c => c.Int());
            AddColumn("dbo.PES_Nomination", "being_AD_Director", c => c.DateTime());
            AddColumn("dbo.PES_Nomination", "Professional_Qualifications", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PES_Nomination", "Professional_Qualifications");
            DropColumn("dbo.PES_Nomination", "being_AD_Director");
            DropColumn("dbo.PES_Nomination", "working_outside_KPMG");
            DropColumn("dbo.PES_Nomination", "working_with_KPMG");
            DropColumn("dbo.PES_Nomination", "Year_joined_KPMG");
        }
    }
}
