namespace NinjaDomain.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ninjas", "DateOfBirh", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ninjas", "DateOfBirh", c => c.DateTime(nullable: false));
        }
    }
}
