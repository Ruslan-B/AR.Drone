using System;
using System.Collections.Generic;

namespace AR.Drone.Client.Configuration
{
    public class ReadOnlyItem<T> : IConfigurationItem
    {
        private readonly string _key;
        private readonly SectionBase _section;

        public ReadOnlyItem(SectionBase section, string name)
        {
            _section = section;
            _key = section.Name + ":" + name;
            section.Configuration.Items.Add(_key, this);
        }

        public T Value { get; protected set; }

        public SectionBase Section
        {
            get { return _section; }
        }

        public string Key
        {
            get { return _key; }
        }

        object IConfigurationItem.Value
        {
            get { return Value; }
        }

        public virtual bool TryUpdate(string value)
        {
            T newValue = Parse(value);
            if (Equals(Value, newValue) == false)
            {
                Value = newValue;
                return true;
            }
            return false;
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
            if (other is T) return Equals((T) other);
            if (other.GetType() != GetType()) return false;
            return Equals((ReadOnlyItem<T>) other);
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

        #region Static

        private static readonly Func<string, T> Parse = CreateParse(typeof (T));

        private static Func<string, T> CreateParse(Type type)
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

        #endregion
    }
}