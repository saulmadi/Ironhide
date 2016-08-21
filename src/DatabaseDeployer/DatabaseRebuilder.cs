using System;
using System.Data.Entity;
using Ironhide.Users.Data;
using Ironhide.Users.Domain.Entities;

namespace DatabaseDeployer
{
    public class DatabaseRebuilder : DropCreateDatabaseAlways<UserDataContext>
    {
        protected override void Seed(UserDataContext context)
        {
            context.Roles.Add(new Role(Guid.NewGuid(), "Guest"));
            context.Roles.Add(new Role(Guid.NewGuid(), "Customer"));
            context.Roles.Add(new Role(Guid.NewGuid(), "Admin"));

            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Create"));
            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Read"));
            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Update"));
            context.UserAbilities.Add(new UserAbility(Guid.NewGuid(), "Delete"));
        }
    }
}