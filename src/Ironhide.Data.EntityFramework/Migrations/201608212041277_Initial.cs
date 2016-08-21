namespace Ironhide.Users.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordResetTokens",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAbilities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        PhoneNumber = c.String(),
                        EncryptedPassword = c.String(),
                        FacebookId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        URL = c.String(),
                        ImageUrl = c.String(),
                        GoogleId = c.String(),
                        FirstName1 = c.String(),
                        LastName1 = c.String(),
                        URL1 = c.String(),
                        ImageUrl1 = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.UserAbilities");
            DropTable("dbo.Roles");
            DropTable("dbo.PasswordResetTokens");
        }
    }
}
