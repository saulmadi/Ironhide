using System;
using System.Linq;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Domain.Services;
using Machine.Specifications;
using Ironhide.App.Data.Specs.Support;

namespace Ironhide.App.Data.Specs.Sample
{
    public class when_creating_a_sample
    {
        static ISampleRepository _repo;
        static readonly Guid SampleId = Guid.NewGuid();
        static TestAppDataContext _dataContext;

        Establish context =
            () =>
            {
                _dataContext = new TestAppDataContext();
                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _repo.Create(new Domain.Entities.Sample(SampleId, "name")).Await();

        It should_be_discoverable =
            () => _dataContext.Samples.Where(x => x.Id == SampleId).ShouldNotBeEmpty();
    }
}