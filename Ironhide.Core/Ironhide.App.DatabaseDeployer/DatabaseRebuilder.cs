using System;
using System.Data.Entity;
using Ironhide.App.Data;
using Ironhide.App.Domain.Entities;

namespace DatabaseDeployer
{
    public class DatabaseRebuilder : DropCreateDatabaseAlways<AppDataContext>
    {
        protected override void Seed(AppDataContext context)
        {
            context.Samples.Add(new Sample(Guid.NewGuid(), "Sample 1"));
            context.Samples.Add(new Sample(Guid.NewGuid(), "Sample 2"));
            context.Samples.Add(new Sample(Guid.NewGuid(), "Sample 3"));
        }
    }
}