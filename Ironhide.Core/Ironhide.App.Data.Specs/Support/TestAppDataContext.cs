using System.Data.Entity;
using System.Threading.Tasks;

namespace Ironhide.App.Data.Specs.Support
{
    public class TestAppDataContext : IAppDataContext
    {
        public TestAppDataContext()
        {
            Samples = new TestDbSet<Domain.Entities.Sample>();

        }

        public int Saved { get; private set; }

        public DbSet<Domain.Entities.Sample> Samples { get; set; }

        public async Task SaveChanges()
        {
            Saved ++;
        }

        public void Seed(params Domain.Entities.Sample[] seed)
        {
            Samples.AddRange(seed);
        }

    }
}