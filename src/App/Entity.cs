using System;

namespace App
{
    public abstract class Entity
    {
        protected Entity() { }

        protected Entity(long id)
        {
            Id = id;
        }

        public long Id { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (GetRealType() != other.GetRealType())
            {
                return false;
            }
            if (this.Id == 0 || other.Id == 0)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (this.GetRealType().ToString() + this.Id).GetHashCode();
        }

        private Type GetRealType()
        {
            var type = this.GetType();

            if (type.ToString().Contains("Castle.Proxies."))
            {
                return type.BaseType;
            }

            return type;
        }
    }
}
