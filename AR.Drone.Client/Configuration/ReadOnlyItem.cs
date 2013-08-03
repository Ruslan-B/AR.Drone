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
            object result;
            if (TryParse(value, out result)) 
            {
                Value = (T) result;
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

        delegate bool TryParseDelegate(string sim, out object result);

        private static readonly TryParseDelegate TryParse = CreateTryParse();

        private static TryParseDelegate CreateTryParse()
        {
            Type type = typeof(T);
            if (type == typeof(string))
                return delegate (string s, out object result) 
                {
                    result = s;
                    return true;
                };

            if (type == typeof (int)) 
                return delegate(string s, out object result) 
                { 
                    int temp;
                    bool success = int.TryParse(s, out temp); 
                    result = temp;
                    return success;
                };

            if (type == typeof (bool))
                return delegate(string s, out object result) 
            { 
                bool temp;
                bool success = bool.TryParse(s.ToLower(), out temp); 
                result = temp;
                return success;
            };

            if (type == typeof (float))
                return delegate(string s, out object result) 
                { 
                    float temp;
                    bool success = float.TryParse(s, out temp); 
                    result = temp;
                    return success;
                };

            if (type == typeof (double))
                return delegate(string s, out object result) 
                { 
                    double temp;
                    bool success = double.TryParse(s, out temp); 
                    result = temp;
                    return success;
                };

            if (type.IsEnum)
                return delegate(string s, out object result) 
                { 
                    result = Enum.Parse(type, s, true); 
                    return true;
                };

            throw new NotSupportedException();
        }

        #endregion
    }
}