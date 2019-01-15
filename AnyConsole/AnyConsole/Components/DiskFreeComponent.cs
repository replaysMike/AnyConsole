using System.IO;
using System.Linq;
using System.Reflection;

namespace AnyConsole.Components
{
    public class DiskFreeComponent : BaseProcessComponent
    {
        private string _value;
        private DriveInfo _drive;

        public DiskFreeComponent() : base()
        {
            var drives = DriveInfo.GetDrives();
            // get the drive the app is using
            var drive = Path.GetPathRoot(Assembly.GetExecutingAssembly().Location);
            _drive = drives.Where(x => x.Name.Equals(drive)).FirstOrDefault();
        }

        public override string Render() => _value;

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            if(_drive != null)
                _value = FormatSize(_drive.AvailableFreeSpace);
        }
    }
}
