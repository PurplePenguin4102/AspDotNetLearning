namespace NinjaDomain.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDoB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ninjas", "DateOfBirh", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ninjas", "DateOfBirh");
        }
    }
}
