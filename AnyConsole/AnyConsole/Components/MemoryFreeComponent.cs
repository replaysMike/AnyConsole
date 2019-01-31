namespace AnyConsole.Components
{
    public class MemoryFreeComponent : BaseProcessComponent
    {
        private string _value;

        public MemoryFreeComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

        public override string Render() => _value;

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            _value = FormatSize(CurrentProcess.PrivateMemorySize64);
        }
    }
}
