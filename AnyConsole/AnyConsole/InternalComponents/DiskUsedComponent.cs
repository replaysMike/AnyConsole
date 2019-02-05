using System.IO;
using System.Linq;
using System.Reflection;

namespace AnyConsole.InternalComponents
{
    public class DiskUsedComponent : BaseProcessComponent
    {
        private string _value;
        private DriveInfo _drive;

        public DiskUsedComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
            var drives = DriveInfo.GetDrives();
            // get the drive the app is using
            var drive = Path.GetPathRoot(Assembly.GetExecutingAssembly().Location);
            _drive = drives.Where(x => x.Name.Equals(drive)).FirstOrDefault();
        }

        public override string Render(object parameters)
        {
            try
            {
                return _value;
            }
            finally
            {
                HasUpdates = false;
            }
        }

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            if (_drive != null)
            {
                var newValue = FormatSize(_drive.TotalSize - _drive.TotalFreeSpace);
                if (!newValue.Equals(_value))
                {
                    _value = newValue;
                    HasUpdates = true;
                }
            }
        }
    }
}
