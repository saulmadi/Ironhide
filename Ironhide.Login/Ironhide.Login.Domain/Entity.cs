using System;

namespace Ironhide.Login.Domain
{
    public abstract class Entity : IEntity
    {
        bool _saved;
        public virtual Guid Id { get; protected set; }

        public virtual void OnSave()
        {
            _saved = true;
        }

        public virtual void OnLoad()
        {
            _saved = true;
        }

        public virtual bool IsPersisted()
        {
            return _saved;
        }
    }
}