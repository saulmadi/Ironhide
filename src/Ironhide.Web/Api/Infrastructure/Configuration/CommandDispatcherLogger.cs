using System;
using System.Linq;
using AcklenAvenue.Commands;
using log4net;

namespace Ironhide.Web.Api.Infrastructure.Configuration
{
    public class CommandDispatcherLogger:ICommandDispatcher
    {
        readonly ICommandDispatcher _decoratedDispatcher;
        readonly ILog _logger;

        public CommandDispatcherLogger(ICommandDispatcher decoratedDispatcher, ILog logger)
        {
            _decoratedDispatcher = decoratedDispatcher;
            _logger = logger;
        }

        public void Dispatch(IUserSession userSession, object command)
        {

            try
            {
             
                _decoratedDispatcher.Dispatch(userSession,command);

               
            }
            catch (Exception e)
            {
                var errorMessage = "1) Error calling " + command.GetType() + "\n";
              
              var properties = command.GetType().GetProperties();
              var propertiesMessage = properties.Aggregate("", (current, property) => current + ("Property Name " + property.Name + " Property Value " + property.GetValue(command)));

              errorMessage += "2) " + propertiesMessage + "\n";
                errorMessage += "3) " + e.Message; 
              _logger.Error(errorMessage);
              
              throw;

            }
        }
    }
}
