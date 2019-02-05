using System.IO;
using System.Linq;
using System.Reflection;

namespace AnyConsole.InternalComponents
{
    public class DiskUsedComponent : BaseProcessComponent
    {
        private string _value;
        private DriveInfo _drive;
        private DriveInfo[] _drives;

        public DiskUsedComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
            _drives = DriveInfo.GetDrives();
        }

        public override string Render(object parameters)
        {
            if (_drive == null)
            {
                if (parameters != null)
                {
                    // get the drive provided by the client
                    if (_drives.Any(x => x.Name.Equals(parameters.ToString(), System.StringComparison.InvariantCultureIgnoreCase)))
                        _drive = _drives.Where(x => x.Name.Equals(parameters.ToString(), System.StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    else
                        throw new System.ArgumentOutOfRangeException($"The drive {parameters.ToString()} is not a valid configuration option. Please choose one of the following: {string.Join(",", _drives.Select(x => x.Name).ToArray())}");
                }
                else
                {
                    // get the drive the app is using as a default                    
                    var applicationDrive = Path.GetPathRoot(Assembly.GetExecutingAssembly().Location);
                    _drive = _drives.Where(x => x.Name.Equals(applicationDrive, System.StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                }
            }

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
