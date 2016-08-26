using System;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Common;
using Ironhide.App.Domain.Application.Commands;
using Ironhide.App.Domain.Services;

namespace Ironhide.App.Domain.Validators
{
    public class SampleValidator : ICommandValidator<IUserSession, AddSample>
    {
        readonly ISampleRepository _readOnlyRepo;
        readonly ITimeProvider _timeProvider;

        public SampleValidator(ISampleRepository readOnlyRepo,
            ITimeProvider timeProvider)
        {
            _readOnlyRepo = readOnlyRepo;
            _timeProvider = timeProvider;
        }

        public async Task Validate(IUserSession userSession, AddSample command)
        {
            if (command.Name.Equals("Name"))
            {
                throw new ArgumentException("Sample name cannot be 'Name'");
            }
        }
    }
}