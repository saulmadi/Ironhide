using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Sample
{
    public class when_finding_multiple_sample_matches_by_any_property
    {
        static SampleRepository _repo;
        static IEnumerable<Domain.Entities.Sample> _result;
        static TestAppDataContext _dataContext;
        static IList<Domain.Entities.Sample> _samples;

        Establish context =
            () =>
            {
                _samples = Builder<Domain.Entities.Sample>.CreateListOfSize(5)
                    .TheLast(3)
                    .With(x => x.Name, "Mickey " + new Random().Next())
                    .Build();

                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_samples.ToArray());

                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.Query<Domain.Entities.Sample>(x => x.Name.StartsWith("Mickey")).WaitForResult();

        It should_return_the_samples_that_match_the_query =
            () => _result.ShouldContainOnly(_samples[2], _samples[3], _samples[4]);
    }
}