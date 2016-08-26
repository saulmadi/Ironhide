using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ironhide.App.Domain.Entities;
using Ironhide.App.Domain.Services;


namespace Ironhide.App.Data.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        readonly IAppDataContext _dataContext;

        public SampleRepository(IAppDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<TSample> First<TSample>(Expression<Func<TSample, bool>> query) where TSample : Sample
        {
            return _dataContext.Samples.OfType<TSample>().FirstAsync(query);
        }

        public async Task<TSample> GetById<TSample>(Guid id) where TSample : Sample
        {
            return await _dataContext.Samples.OfType<TSample>().FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TSample>> GetAll<TSample>() where TSample : Sample
        {
            return await _dataContext.Samples.OfType<TSample>().ToListAsync();
        }

        public async Task<IEnumerable<TSample>> Query<TSample>(Expression<Func<TSample, bool>> expression) where TSample : Sample
        {
            return await _dataContext.Samples.OfType<TSample>().Where(expression).ToListAsync();
        }

        public async Task Delete(Guid userId)
        {
            Sample sample = await _dataContext.Samples.FindAsync(userId);
            _dataContext.Samples.Remove(sample);
            await _dataContext.SaveChanges();
        }

        public async Task Update(Sample user)
        {
            await _dataContext.SaveChanges();
        }

        public async Task<Sample> Create(Sample sample)
        {
            _dataContext.Samples.Add(sample);
            await _dataContext.SaveChanges();
            return sample;
        }
    }
}