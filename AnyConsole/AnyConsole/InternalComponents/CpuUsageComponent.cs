using static AnyConsole.ExtendedConsole;

namespace AnyConsole.InternalComponents
{
    public class CpuUsageComponent : BaseProcessComponent
    {
        private string _value;

        public CpuUsageComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

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
            var newValue = $"{CurrentProcess.TotalProcessorTime}";
            if (!newValue.Equals(_value))
            {
                _value = newValue;
                HasUpdates = true;
            }
        }
    }
}
