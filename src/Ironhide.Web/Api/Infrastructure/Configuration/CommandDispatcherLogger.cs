using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using log4net;

namespace Ironhide.Web.Api.Infrastructure.Configuration
{
    public class CommandDispatcherLogger : ICommandDispatcher
    {
        readonly ICommandDispatcher _decoratedDispatcher;
        readonly ILog _logger;

        public CommandDispatcherLogger(ICommandDispatcher decoratedDispatcher, ILog logger)
        {
            _decoratedDispatcher = decoratedDispatcher;
            _logger = logger;
        }

        public async Task Dispatch(IUserSession userSession, object command)
        {
            try
            {
                await _decoratedDispatcher.Dispatch(userSession, command);
            }
            catch (Exception e)
            {
                string errorMessage = "1) Error calling " + command.GetType() + "\n";

                PropertyInfo[] properties = command.GetType().GetProperties();
                string propertiesMessage = properties.Aggregate("",
                    (current, property) =>
                        current + ("Property Name " + property.Name + " Property Value " + property.GetValue(command)));

                errorMessage += "2) " + propertiesMessage + "\n";
                errorMessage += "3) " + e.Message;
                _logger.Error(errorMessage);

                throw;
            }
        }
    }
}