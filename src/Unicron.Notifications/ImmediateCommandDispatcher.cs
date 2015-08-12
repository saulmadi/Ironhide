using System;
using System.Collections.Generic;
using System.Linq;
using BlingBag;

namespace Starscream.Notifications
{
    public class ImmediateCommandDispatcher : SynchronousCommandDispatcherBase
    {
        readonly IBlingInitializer<DomainEvent> _blingInitializer;
        readonly IEnumerable<ICommandHandler> _commandHandlers;
        readonly IEnumerable<ICommandValidator> _commandValidators;

        public ImmediateCommandDispatcher(IBlingInitializer<DomainEvent> blingInitializer, IEnumerable<ICommandHandler> commandHandlers, IEnumerable<ICommandValidator> commandValidators)
        {
            _blingInitializer = blingInitializer;
            _commandHandlers = commandHandlers;
            _commandValidators = commandValidators;
        }

        protected override void InitializeHandler(object handler)
        {
            _blingInitializer.Initialize(handler);
        }

        protected override IEnumerable<object> FindHandlers(Type genericCommandType)
        {
            return _commandHandlers.Where(x => Enumerable.Any<Type>(x.GetType().GetInterfaces(), i => i == genericCommandType));
        }

        protected override IEnumerable<object> FindValidators(Type genericCommandValidatorType)
        {
            return _commandValidators.Where(x => Enumerable.Any<Type>(x.GetType().GetInterfaces(), i => i == genericCommandValidatorType));
        }
    }
}