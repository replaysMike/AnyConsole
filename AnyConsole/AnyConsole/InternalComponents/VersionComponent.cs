using System.Reflection;
using System.Text.RegularExpressions;
using static AnyConsole.ExtendedConsole;

namespace AnyConsole.InternalComponents
{
    public class VersionComponent : BaseProcessComponent
    {
        private string _value;
        private string _format = "Major.Minor.Revision";

        public VersionComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

        public override string Render(object parameters)
        {
            try
            {
                if (parameters != null && parameters.GetType() == typeof(string))
                {
                    _format = parameters.ToString();
                }
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
            // get the version of the entry assembly
            var version = Assembly.GetEntryAssembly().GetName().Version;
            var newValue = _format;
            newValue = Regex.Replace(newValue, "major", version.Major.ToString(), RegexOptions.IgnoreCase);
            newValue = Regex.Replace(newValue, "minor", version.Minor.ToString(), RegexOptions.IgnoreCase);
            newValue = Regex.Replace(newValue, "revision", version.Revision.ToString(), RegexOptions.IgnoreCase);
            newValue = Regex.Replace(newValue, "build", version.Build.ToString(), RegexOptions.IgnoreCase);
            if (!newValue.Equals(_value))
            {
                _value = newValue;
                HasUpdates = true;
            }
        }
    }
}
