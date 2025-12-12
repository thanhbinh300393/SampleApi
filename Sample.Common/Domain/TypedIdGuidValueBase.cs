using System;

namespace Sample.Common.Domain
{
    public abstract class TypedIdGuidValueBase : IEquatable<TypedIdGuidValueBase>
    {
        public Guid Value { get; }

        protected TypedIdGuidValueBase(Guid value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypedIdGuidValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(TypedIdGuidValueBase other)
        {
            return this.Value == other.Value;
        }

        public static bool operator ==(TypedIdGuidValueBase obj1, TypedIdGuidValueBase obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                {
                    return true;
                }
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(TypedIdGuidValueBase x, TypedIdGuidValueBase y)
        {
            return !(x == y);
        }
    }
}