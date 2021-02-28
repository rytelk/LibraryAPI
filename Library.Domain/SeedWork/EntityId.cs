
namespace Library.Domain.SeedWork
{
    public abstract class EntityId
    {
        public int Id { get; set; }

        public bool IsTransient()
        {
            return Id == default(int);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EntityId))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (EntityId)obj;
            if (item.IsTransient() || IsTransient())
                return false;

            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(EntityId left, EntityId right)
        {
            return Equals(left, null) ? Equals(right, null) : left.Equals(right);
        }

        public static bool operator !=(EntityId left, EntityId right)
        {
            return !(left == right);
        }
    }
}