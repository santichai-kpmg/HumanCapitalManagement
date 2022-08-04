namespace HumanCapitalManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _300520191027 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Perdiem_Transport", "Type_of_withdrawal", c => c.String(maxLength: 5));
            AddColumn("dbo.Perdiem_Transport", "Company", c => c.String(maxLength: 100));
            AddColumn("dbo.Perdiem_Transport", "Reimbursable", c => c.String(maxLength: 10));
            AddColumn("dbo.Perdiem_Transport", "Business_Purpose", c => c.String(maxLength: 1000));
            AddColumn("dbo.Perdiem_Transport", "Description", c => c.String(maxLength: 1000));
            AddColumn("dbo.Perdiem_Transport", "Amount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Perdiem_Transport", "Amount");
            DropColumn("dbo.Perdiem_Transport", "Description");
            DropColumn("dbo.Perdiem_Transport", "Business_Purpose");
            DropColumn("dbo.Perdiem_Transport", "Reimbursable");
            DropColumn("dbo.Perdiem_Transport", "Company");
            DropColumn("dbo.Perdiem_Transport", "Type_of_withdrawal");
        }
    }
}
