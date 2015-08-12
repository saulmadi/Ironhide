namespace Starscream.Notifications
{
    public interface ICommandValidator
    {
    }

    public interface ICommandValidator<in T> : ICommandValidator
    {
        void Validate(IUserSession userSession, T command);
    }
}