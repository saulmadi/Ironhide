using System;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Sample
{
    public class when_getting_one_sample_by_id
    {
        static SampleRepository _repo;
        static Domain.Entities.Sample _result;
        static TestAppDataContext _dataContext;
        static Domain.Entities.Sample _exactSampleByReference;
        static Guid _userId;

        Establish context =
            () =>
            {
                _userId = Guid.NewGuid();
                _exactSampleByReference = new Domain.Entities.Sample(_userId, "name");
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_exactSampleByReference);
                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetById<Domain.Entities.Sample>(_userId).Await();

        It should_return_the_matching_sample_by_id =
            () => _result.ShouldEqual(_exactSampleByReference);
    }
}