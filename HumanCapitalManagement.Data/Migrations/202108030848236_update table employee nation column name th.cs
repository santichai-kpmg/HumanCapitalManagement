namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableemployeenationcolumnnameth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_middle", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_name_th", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_lastname_th", c => c.String(maxLength: 500));
            AddColumn("dbo.TM_EmployeeForeign_Visa", "em_telephone", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_telephone");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_lastname_th");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_name_th");
            DropColumn("dbo.TM_EmployeeForeign_Visa", "em_middle");
        }
    }
}
