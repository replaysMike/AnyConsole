namespace AnyConsole.InternalComponents
{
    public class MemoryFreeComponent : BaseProcessComponent
    {
        private string _value;

        public MemoryFreeComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

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
            if (tickCount % 60 == 0)
            {
                var newValue = FormatSize(CurrentProcess.PrivateMemorySize64);
                if (!newValue.Equals(_value))
                {
                    _value = newValue;
                    HasUpdates = true;
                }
            }
        }
    }
}
