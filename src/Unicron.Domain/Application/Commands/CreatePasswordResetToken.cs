namespace Unicron.Users.Domain.Application.Commands
{
    public class CreatePasswordResetToken
    {
        public string Email { get; private set; }

        public CreatePasswordResetToken(string email)
        {
            Email = email;
        }
    }
}