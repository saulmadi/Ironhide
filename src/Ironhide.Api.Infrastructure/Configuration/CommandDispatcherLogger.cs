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

        public void LogInfo(IUserSession userSession, object sender, DateTime timeStamp, string message)
        {
            _logger.InfoFormat("{0}: {1}", sender.GetType().Name, message);
        }

        public void LogException(IUserSession userSession, object sender, DateTime timeStamp, Exception exception)
        {
            
        }

        public void LogInfo(IUserSession userSession, object sender, DateTime timeStamp, string message, object command)
        {
            throw new NotImplementedException();
        }

        public void LogException(IUserSession userSession, object sender, DateTime timeStamp, Exception exception, object command)
        {
            var commandTransformed = CommandCasted(command);
            if (commandTransformed != null)
            {
                SetGlobalContextForLogger(userSession, commandTransformed, "Error");

                _logger.Error(
                    $"Failed {sender.GetType().Name}:by reason: {exception.Message} with the following data {commandTransformed.MessageToLog}",
                    exception);
            }
            else
            {
                throw new InvalidCastException("The command can't be logged");
            }
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
    }
}