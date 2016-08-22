namespace Ironhide.Users.Domain.Entities
{
    public class ProfileAdministrator : IProfile
    {
        public ProfileAdministrator()
        {
            Name = "Administrador";
        }

        public virtual string Name { get; protected set; }
    }

    public interface IProfile
    {
        string Name { get; }
    }
}