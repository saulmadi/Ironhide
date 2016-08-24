using System;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Sample
{
    public class when_finding_one_sample_match_by_any_property
    {
        const string Name = "Mickey Mouse";
        static SampleRepository _repo;
        static Domain.Entities.Sample _result;
        static TestAppDataContext _dataContext;
        static Domain.Entities.Sample _exactSampleByReference;

        Establish context =
            () =>
            {
                _exactSampleByReference = new Domain.Entities.Sample(Guid.NewGuid(), Name);
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_exactSampleByReference);

                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.First<Domain.Entities.Sample>(x => x.Name == Name).Await();

        It should_return_the_matching_sample_based_on_the_query =
            () => _result.ShouldEqual(_exactSampleByReference);
    }
}