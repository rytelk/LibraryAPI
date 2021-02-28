using System.Collections.Generic;
using System.Linq;

namespace Library.Domain.SeedWork
{
    public abstract class ValueObject
    {
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            return Equals(a, null) ? Equals(b, null) : a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }
    }
}