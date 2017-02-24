namespace NinjaDomain.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedOldDT : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ninjas", "DateOfBirh");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ninjas", "DateOfBirh", c => c.DateTime());
        }
    }
}
