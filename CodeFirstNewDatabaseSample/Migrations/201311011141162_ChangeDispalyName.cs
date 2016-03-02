namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDispalyName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Users", name: "DisplayName", newName: "Display_Name");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Users", name: "Display_Name", newName: "DisplayName");
        }
    }
}
