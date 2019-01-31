using System.Diagnostics;

namespace AnyConsole.Components
{
    public class BaseProcessComponent : IComponent
    {
        public bool HasUpdates => true;

        public bool HasCustomThreadManagement => false;

        public Process CurrentProcess { get { return Process.GetCurrentProcess(); } }

        private ConsoleDataContext _data;
        internal ConsoleDataContext _consoleDataContext;

        public BaseProcessComponent(ConsoleDataContext consoleDataContext)
        {
            _consoleDataContext = consoleDataContext;
        }

        public virtual string Render()
        {
            return string.Empty;
        }

        public virtual void Setup(ConsoleDataContext data, string name, IExtendedConsole console)
        {
            _data = data;
        }

        public virtual void Tick(ulong tickCount)
        {
            
        }

        /// <summary>
        /// Format bytes into human readable size
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string FormatSize(long length)
        {
            // Get absolute value
            long absolute_i = (length < 0 ? -length : length);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (length >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (length >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (length >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (length >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (length >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = length;
            }
            else
            {
                return length.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.### ") + suffix;
        }
    }
}
