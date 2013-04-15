using System;
using AR.Drone.Client.Commands;

namespace AR.Drone.Client.Configuration
{
    public static class CommandHelper
    {
        public static ATCommand ToCommand<T>(this ReadWriteItem<T> item)
        {
            Type type = typeof (T);

            if (type == typeof (string))
                return new ConfigCommand(item.Key, (string) (object) item.Value);

            if (type == typeof (int))
                return new ConfigCommand(item.Key, (int) (object) item.Value);

            if (type == typeof (bool))
                return new ConfigCommand(item.Key, (bool) (object) item.Value);

            if (type == typeof (float))
                return new ConfigCommand(item.Key, (float) (object) item.Value);

            if (type.IsEnum)
                return new ConfigCommand(item.Key, (Enum) (object) item.Value);

            throw new NotSupportedException();
        }
    }
}