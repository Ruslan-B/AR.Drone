using System;
using System.Collections.Generic;

namespace AR.Drone.Client.Configuration
{
    public class ReadOnlyItem<T> : IConfigurationItem
    {
        private readonly string _key;
        private readonly Func<string, T> _parse;

        public ReadOnlyItem(string key)
        {
            _key = key;
            _parse = CreateParser(typeof (T));
        }

        public T Value { get; protected set; }

        public string Key
        {
            get { return _key; }
        }

        object IConfigurationItem.Value
        {
            get { return Value; }
        }

        public bool TryUpdate(string value)
        {
            T newValue = _parse(value);
            if (Equals(Value, newValue) == false)
            {
                Value = newValue;
                return true;
            }
            return false;
        }

        private static Func<string, T> CreateParser(Type type)
        {
            if (type == typeof (string))
                return v => (T) (object) v;

            if (type == typeof (int))
                return v => (T) (object) int.Parse(v);

            if (type == typeof (bool))
                return v => (T) (object) bool.Parse(v);

            if (type == typeof (float))
                return v => (T) (object) float.Parse(v);

            if (type == typeof (double))
                return v => (T) (object) double.Parse(v);

            if (type.IsEnum)
                return v => (T) Enum.Parse(type, v);

            throw new NotSupportedException();
        }

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        protected bool Equals(ReadOnlyItem<T> other)
        {
            return string.Equals(_key, other._key) && Equals(Value, other.Value);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is T) return Equals((T)other);
            if (other.GetType() != GetType()) return false;
            return Equals((ReadOnlyItem<T>)other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public static bool operator ==(ReadOnlyItem<T> left, T right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReadOnlyItem<T> left, T right)
        {
            return !(left == right);
        }
    }
}