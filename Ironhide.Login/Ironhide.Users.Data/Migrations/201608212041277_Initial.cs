using System.Data.Entity.Migrations;

namespace Ironhide.Users.Data.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordResetTokens",
                c => new
                     {
                         Id = c.Guid(false),
                         UserId = c.Guid(false),
                         Created = c.DateTime(false),
                     })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Roles",
                c => new
                     {
                         Id = c.Guid(false),
                         Description = c.String(),
                     })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserAbilities",
                c => new
                     {
                         Id = c.Guid(false),
                         Description = c.String(),
                     })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                     {
                         Id = c.Guid(false),
                         Name = c.String(),
                         Email = c.String(),
                         IsActive = c.Boolean(false),
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
                         Discriminator = c.String(false, 128),
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