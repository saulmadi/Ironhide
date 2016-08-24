using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ironhide.App.Domain.Entities;

namespace Ironhide.App.Domain.Services
{
    public interface ISampleRepository
    {
        Task<TSample> First<TSample>(Expression<Func<TSample, bool>> query) where TSample : Sample;
        Task<TSample> GetById<TSample>(Guid id) where TSample : Sample;
        Task<IEnumerable<TSample>> GetAll<TSample>() where TSample : Sample;
        Task<IEnumerable<TSample>> Query<TSample>(Expression<Func<TSample, bool>> expression) where TSample : Sample;
        Task Delete(Guid id);
        Task Update(Sample sample);
        Task<Sample> Create(Sample sample);
    }
}