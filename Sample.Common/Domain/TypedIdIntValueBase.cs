using System;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Common.Domain
{
    public abstract class TypedIdIntValueBase : IEquatable<TypedIdIntValueBase>
    {
        public int Value { get; }

        protected TypedIdIntValueBase(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypedIdIntValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals([AllowNull] TypedIdIntValueBase other)
        {
            return this.Value == other.Value;
        }

        public static bool operator ==(TypedIdIntValueBase obj1, TypedIdIntValueBase obj2)
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
        public static bool operator !=(TypedIdIntValueBase x, TypedIdIntValueBase y)
        {
            return !(x == y);
        }
    }
}