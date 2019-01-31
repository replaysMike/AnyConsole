namespace AnyConsole.Components
{
    public class MemoryUsedComponent : BaseProcessComponent
    {
        private string _value;

        public MemoryUsedComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

        public override string Render() => _value;

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            _value = FormatSize(CurrentProcess.WorkingSet64);
        }
    }
}
