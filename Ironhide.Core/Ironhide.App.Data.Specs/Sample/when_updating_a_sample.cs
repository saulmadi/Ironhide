using System;
using System.Linq;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.App.Domain.Services;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Sample
{
    public class when_updating_a_sample
    {
        static ISampleRepository _repo;
        static readonly Guid UserId = Guid.NewGuid();
        static TestAppDataContext _dataContext;
        static Domain.Entities.Sample _sampleToUpdate;

        Establish context =
            () =>
            {
                _dataContext = new TestAppDataContext();
                _sampleToUpdate = new Domain.Entities.Sample(UserId, "name");
                _dataContext.Seed(_sampleToUpdate);

                _repo = new SampleRepository(_dataContext);

                _sampleToUpdate.ChangeName("new name");
            };

        Because of =
            () => _repo.Update(_sampleToUpdate).Await();

        It should_change_the_values_in_the_database =
            () => _dataContext.Samples.First(x => x.Id == UserId).Name.ShouldEqual("new name");
    }
}