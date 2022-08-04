namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update110220191424 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_WorkExperience", "base_salary", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_WorkExperience", "transportation", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_WorkExperience", "mobile_allowance", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_WorkExperience", "position_allowance", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_WorkExperience", "other_allowance", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_WorkExperience", "annual_leave", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_WorkExperience", "variable_bonus", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TM_WorkExperience", "expected_salary", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TM_WorkExperience", "CompanyName", c => c.String(maxLength: 500));
            AlterColumn("dbo.TM_WorkExperience", "JobPosition", c => c.String(maxLength: 500));
            AlterColumn("dbo.TM_WorkExperience", "TypeOfRelatedToJob", c => c.String(maxLength: 10));
            DropColumn("dbo.TM_WorkExperience", "TotalYearsOfWorkExperience");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TM_WorkExperience", "TotalYearsOfWorkExperience", c => c.String());
            AlterColumn("dbo.TM_WorkExperience", "TypeOfRelatedToJob", c => c.String());
            AlterColumn("dbo.TM_WorkExperience", "JobPosition", c => c.String());
            AlterColumn("dbo.TM_WorkExperience", "CompanyName", c => c.String());
            DropColumn("dbo.TM_WorkExperience", "expected_salary");
            DropColumn("dbo.TM_WorkExperience", "variable_bonus");
            DropColumn("dbo.TM_WorkExperience", "annual_leave");
            DropColumn("dbo.TM_WorkExperience", "other_allowance");
            DropColumn("dbo.TM_WorkExperience", "position_allowance");
            DropColumn("dbo.TM_WorkExperience", "mobile_allowance");
            DropColumn("dbo.TM_WorkExperience", "transportation");
            DropColumn("dbo.TM_WorkExperience", "base_salary");
        }
    }
}
