using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Starscream.Notifications
{
    public abstract class SynchronousCommandDispatcherBase : ICommandDispatcher
    {
        public void Dispatch(IUserSession userSession, object command)
        {
            ValidateTheCommand(userSession, command);
            HandleTheCommand(userSession, command);
        }

        void HandleTheCommand(IUserSession userSession, object command)
        {
            IEnumerable matchingCommandHandlers = GetMatchingCommandHandlers(command);

            foreach (object handler in matchingCommandHandlers)
            {
                InitializeHandler(handler);
              
                    InvokeMethod("Handle", handler, userSession, command);
               
            }
        }

        void ValidateTheCommand(IUserSession userSession, object command)
        {
            foreach (object validator in GetMatchingCommandValidators(command))
            {
                InvokeMethod("Validate", validator, userSession, command);
            }
        }

        static void InvokeMethod(string methodName, object invokableObject, params object[] methodArgs)
        {
            MethodInfo handlerMethod = invokableObject.GetType().GetMethod(methodName);
            try
            {
                handlerMethod.Invoke(invokableObject, methodArgs);
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        IEnumerable<object> GetMatchingCommandHandlers(object command)
        {
            Type handlerType = typeof (ICommandHandler<>);
            Type genericHandlerType = handlerType.MakeGenericType(command.GetType());
            List<object> matchingCommandHandlers = FindHandlers(genericHandlerType).ToList();

            if (!matchingCommandHandlers.Any())
                throw new NotImplementedException(string.Format("No command handlers could be found to handle '{0}'.",
                    command.GetType()));

            return matchingCommandHandlers;
        }

        IEnumerable GetMatchingCommandValidators(object command)
        {
            Type handlerType = typeof (ICommandValidator<>);
            Type genericHandlerType = handlerType.MakeGenericType(command.GetType());
            return FindValidators(genericHandlerType);
        }

        protected abstract void InitializeHandler(object handler);
        protected abstract IEnumerable<object> FindHandlers(Type genericCommandHandlerType);
        protected abstract IEnumerable<object> FindValidators(Type genericCommandValidatorType);
    }
}