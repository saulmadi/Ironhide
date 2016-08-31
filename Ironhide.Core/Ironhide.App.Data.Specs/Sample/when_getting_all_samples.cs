using System;
using System.Collections.Generic;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Sample
{
    public class when_getting_all_samples
    {
        static SampleRepository _repo;
        static IEnumerable<Domain.Entities.Sample> _result;
        static TestAppDataContext _dataContext;
        static Domain.Entities.Sample _exactSampleByReference1;
        static Guid _userId1;

        Establish context =
            () =>
            {
                _userId1 = Guid.NewGuid();
                _exactSampleByReference1 = new Domain.Entities.Sample(_userId1, "name");
                _userId2 = Guid.NewGuid();
                _exactSampleByReference2 = new Domain.Entities.Sample(_userId2, "name");
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_exactSampleByReference1, _exactSampleByReference2);
                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetAll<Domain.Entities.Sample>().WaitForResult();

        It should_return_the_matching_sample_by_id =
            () => _result.ShouldContainOnly(_exactSampleByReference1, _exactSampleByReference2);

        static Guid _userId2;
        static Domain.Entities.Sample _exactSampleByReference2;
    }
}