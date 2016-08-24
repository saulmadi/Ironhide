using System;
using System.Linq;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.App.Domain.Services;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Sample
{
    public class when_deleting_a_sample
    {
        static ISampleRepository _repo;
        static readonly Guid SampleId = Guid.NewGuid();
        static TestAppDataContext _dataContext;

        Establish context =
            () =>
            {
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(new Domain.Entities.Sample(SampleId, "name"));
                _dataContext.Seed(new Domain.Entities.Sample(Guid.NewGuid(), "another name"));

                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _repo.Delete(SampleId).Await();

        It should_leave_other_samples_alone =
            () => _dataContext.Samples.First(x => x.Name == "another name").ShouldNotBeNull();

        It should_remove_the_sample_from_the_database =
            () => _dataContext.Samples.Where(x => x.Id == SampleId).ShouldBeEmpty();
    }
}