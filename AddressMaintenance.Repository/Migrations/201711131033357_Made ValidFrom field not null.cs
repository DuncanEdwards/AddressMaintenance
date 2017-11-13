namespace AddressMaintenance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeValidFromfieldnotnull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "ValidFrom", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "ValidFrom", c => c.DateTime());
        }
    }
}
