using Autofac.Extras.DynamicProxy2;

namespace Starscream.Notifications
{
   
    public interface ICommandDispatcher
    {
        void Dispatch(IUserSession userSession, object command);
    }
}