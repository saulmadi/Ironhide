using System;
using AcklenAvenue.Commands;
using log4net;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class CommandDispatcherLogger : ICommandDispatcherLogger
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

        ICommandLogged CommandCasted(object command)
        {
            var commandTransformed = command as ICommandLogged;

            return commandTransformed;
        }

        public interface ICommandLogged
        {
            string Id { get; }
            DateTime DateIssued { get; }
            string MessageToLog { get; }
        }

        void SetGlobalContextForLogger(IUserSession userSession, ICommandLogged commandTransformed, string typeOfEvent)
        {
            SetGlobalContextForLogger(Guid.NewGuid(), commandTransformed.Id, userSession.UserIdentifier, typeOfEvent);
        }

        void SetGlobalContextForLogger(Guid commandId, string eventId, string user, string typeOfEvent)
        {
            GlobalContext.Properties["id"] = commandId;
            GlobalContext.Properties["commandId"] = eventId;
            GlobalContext.Properties["userRequestCommand"] = user;
            GlobalContext.Properties["typeOfEvent"] = typeOfEvent;
        }

        string GetTypeOfEvent(string message)
        {
            return message.StartsWith("Starting Synchronously")
                ? "Starting "
                : message.StartsWith("Synchronously dispatching")
                    ? " Requested "
                    : message.StartsWith("Started synchronously validating") ? "Validating " : "Finishing ";
        }
    }
}