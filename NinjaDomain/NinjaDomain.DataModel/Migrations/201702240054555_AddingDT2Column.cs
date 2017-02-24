namespace NinjaDomain.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDT2Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ninjas", "Dummy", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ninjas", "Dummy");
        }
    }
}
