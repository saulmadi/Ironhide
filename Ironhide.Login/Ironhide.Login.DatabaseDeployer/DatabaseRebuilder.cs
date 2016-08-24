using System;
using System.Data.Entity;
using Ironhide.Login.Domain.Entities;
using Ironhide.Users.Data;
using Ironhide.Users.Domain.Entities;

namespace DatabaseDeployer
{
    public class DatabaseRebuilder : DropCreateDatabaseAlways<LoginDataContext>
    {
        protected override void Seed(LoginDataContext context)
        {
            context.Roles.Add(new Role(Guid.NewGuid(), "Basic"));
            context.Roles.Add(new Role(Guid.NewGuid(), "Customer"));
            context.Roles.Add(new Role(Guid.NewGuid(), "Admin"));

            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Create"));
            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Read"));
            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Update"));
            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Delete"));
        }
    }
}