namespace AddressMaintenance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addressinfrastructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AddressLine1 = c.String(nullable: false, maxLength: 300),
                        AddressLine2 = c.String(nullable: false, maxLength: 300),
                        AddressLine3 = c.String(nullable: false, maxLength: 300),
                        PostCode = c.String(nullable: false, maxLength: 8),
                        ValidUntil = c.DateTime(),
                        ValidFrom = c.DateTime(),
                        Customer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Addresses", new[] { "Customer_Id" });
            DropTable("dbo.Addresses");
        }
    }
}
