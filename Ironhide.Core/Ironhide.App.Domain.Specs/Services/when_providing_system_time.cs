using System;
using Ironhide.App.Domain.Services;
using Machine.Specifications;

namespace Ironhide.App.Domain.Specs.Services
{
    public class when_providing_system_time
    {
        static SystemTimeProvider _systemTimeProvider;
        static DateTime _currentDateTime;
        static DateTime _result;

        Establish context =
            () =>
            {
                _systemTimeProvider = new SystemTimeProvider();
                _currentDateTime = DateTime.Now;
            };

        Because of =
            () => _result = _systemTimeProvider.Now();

        It should_return_current_time =
            () => { _result.ShouldBeCloseTo(_currentDateTime, new TimeSpan(0, 0, 5)); };
    }
}