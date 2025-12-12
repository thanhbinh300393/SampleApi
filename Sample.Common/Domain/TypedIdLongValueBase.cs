using System;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Common.Domain
{
    public abstract class TypedIdLongValueBase : IEquatable<TypedIdLongValueBase>
    {
        public long Value { get; }

        protected TypedIdLongValueBase(long value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypedIdLongValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals([AllowNull] TypedIdLongValueBase other)
        {
            return this.Value == other.Value;
        }

        public static bool operator ==(TypedIdLongValueBase obj1, TypedIdLongValueBase obj2)
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
        public static bool operator !=(TypedIdLongValueBase x, TypedIdLongValueBase y)
        {
            return !(x == y);
        }
    }
}