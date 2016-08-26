using System;
using System.Collections.Generic;

namespace Ironhide.Login.Domain.Entities
{
    public class User : Entity
    {
        IEnumerable<UserAbility> _userAbilities = new List<UserAbility>();
        IEnumerable<Role> _userRoles = new List<Role>();

        public User(Guid id, string name, string email)
        {
            Name = name;
            Email = email;
            Id = id;
            IsActive = true;
        }


        protected User()
        {
        }

        public virtual string Name { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual bool IsActive { get; protected set; }

        public virtual IEnumerable<Role> UserRoles
        {
            get { return _userRoles; }
            protected set { _userRoles = value; }
        }

        public virtual IEnumerable<UserAbility> UserAbilities
        {
            get { return _userAbilities; }
            protected set { _userAbilities = value; }
        }

    }
}