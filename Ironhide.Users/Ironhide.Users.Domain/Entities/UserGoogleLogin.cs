using System;

namespace Ironhide.Users.Domain.Entities
{
    public class UserGoogleLogin : User
    {
        protected UserGoogleLogin()
        {
        }

        public UserGoogleLogin(Guid id, string name, string email, string googleId, string firstName, string lastName,
            string imageUrl, string url) : base(id, name, email)
        {
            GoogleId = googleId;
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
            URL = url;
        }

        public virtual string GoogleId { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string URL { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
    }
}