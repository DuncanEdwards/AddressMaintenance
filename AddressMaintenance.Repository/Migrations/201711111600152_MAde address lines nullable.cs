namespace AddressMaintenance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MAdeaddresslinesnullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "AddressLine2", c => c.String(nullable: false));
            AlterColumn("dbo.Addresses", "AddressLine3", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "AddressLine3", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Addresses", "AddressLine2", c => c.String(nullable: false, maxLength: 300));
        }
    }
}
