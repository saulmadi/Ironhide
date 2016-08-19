using System;
using AcklenAvenue.Commands;
using BlingBag;
using log4net;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class CommandDispatcherLogger : ICommandDispatcherLogger, IBlingLogger
    {
        readonly ILog _logger;

        public CommandDispatcherLogger(ILog logger)
        {
            _logger = logger;
        }

        public void LogInfo(IUserSession userSession, object sender, DateTime timeStamp, string message, object command)
        {
            _logger.InfoFormat("{0}: {1}", sender.GetType().Name, message);
        }

        public void LogException(IUserSession userSession, object sender, DateTime timeStamp, Exception exception, object command)
        {
           
        }

        public interface ICommandLogged
        {
            string Id { get; }
            DateTime DateIssued { get; }
            string MessageToLog { get; }
        }

        public void LogException(object handler, DateTime timeStamp, Exception exception)
        {
            var errorMessage = "Error handling command'" + handler.GetType().Name + "'\n";
            errorMessage += "2) " + exception.Message;
            _logger.Error(errorMessage, exception);
        }

        public void LogInfo(object handler, DateTime timeStamp, string message)
        {
            _logger.InfoFormat("{0}: {1}", handler.GetType().Name, message);
        }
    }
}