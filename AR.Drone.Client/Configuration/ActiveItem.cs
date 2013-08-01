using System;
using AR.Drone.Client.Commands;

namespace AR.Drone.Client.Configuration
{
    public class ActiveItem<T> : ReadOnlyItem<T>
    {
        public ActiveItem(SectionBase section, string key)
            : base(section, key)
        {
        }

        public void ChangeTo(T value)
        {
            Value = value;

            ATCommand command = CreateCommand(this);
            Section.Configuration.Enqueue(command);
        }

        #region Static

        private static readonly Func<ActiveItem<T>, ATCommand> CreateCommand = CreateToCommand(typeof (T));


        private static Func<ActiveItem<T>, ATCommand> CreateToCommand(Type type)
        {
            if (type == typeof (string))
                return item => new ConfigCommand(item.Key, (string) (object) item.Value);

            if (type == typeof (int))
                return item => new ConfigCommand(item.Key, (int) (object) item.Value);

            if (type == typeof (bool))
                return item => new ConfigCommand(item.Key, (bool) (object) item.Value);

            if (type == typeof (float))
                return item => new ConfigCommand(item.Key, (float) (object) item.Value);

            if (type.IsEnum)
                return item => new ConfigCommand(item.Key, (Enum) (object) item.Value);

            throw new NotSupportedException();
        }

        #endregion
    }
}