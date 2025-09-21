using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.Common
{
    public abstract record EntityId<T> where T : EntityId<T>
    {
        public Guid Value { get; }

        protected EntityId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("EntityId cannot be empty.", nameof(value));

            Value = value;
        }

        public static implicit operator Guid(EntityId<T> id) => id.Value;
        public static implicit operator EntityId<T>(Guid value) => (T)Activator.CreateInstance(typeof(T), value)!;

        public override string ToString() => Value.ToString();
    }
}
