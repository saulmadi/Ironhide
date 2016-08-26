namespace Ironhide.Users.Domain.Application.Commands
{
    public class StartPasswordResetProcess
    {
        public StartPasswordResetProcess(string email)
        {
            Email = email;
        }

        public string Email { get; private set; }
    }
}