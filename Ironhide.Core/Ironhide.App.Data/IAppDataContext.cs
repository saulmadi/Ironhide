using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.App.Domain.Entities;

namespace Ironhide.App.Data
{
    public interface IAppDataContext
    {
        DbSet<Sample> Samples { get; set; }
        Task SaveChanges();
    }
}