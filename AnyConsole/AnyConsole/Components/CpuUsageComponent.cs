using static AnyConsole.ExtendedConsole;

namespace AnyConsole.Components
{
    public class CpuUsageComponent : BaseProcessComponent
    {
        private string _value;

        public CpuUsageComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

        public override string Render() => _value;

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            _value = $"{CurrentProcess.TotalProcessorTime}";
        }
    }
}
