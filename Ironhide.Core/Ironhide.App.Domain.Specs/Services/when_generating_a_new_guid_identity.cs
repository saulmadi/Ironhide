using System;
using Ironhide.App.Domain.Services;
using Machine.Specifications;

namespace Ironhide.App.Domain.Specs.Services
{
    public class when_generating_a_new_guid_identity
    {
        static GuidIdentityGenerator _identityGenerator;
        static object _result;

        Establish context =
            () => { _identityGenerator = new GuidIdentityGenerator(); };

        Because of =
            () => _result = _identityGenerator.Generate();

        It should_generate_a_new_guid =
            () => { _result.ShouldBeAssignableTo(typeof (Guid)); };
    }
}