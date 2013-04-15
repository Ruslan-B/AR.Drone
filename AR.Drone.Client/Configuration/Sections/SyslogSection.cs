using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class SyslogSection
    {
        public readonly ReadWriteItem<int> Output = new ReadWriteItem<int>("syslog:output");
        public readonly ReadWriteItem<int> MaxSize = new ReadWriteItem<int>("syslog:max_size");
        public readonly ReadWriteItem<int> NbFiles = new ReadWriteItem<int>("syslog:nb_files");
    }
}