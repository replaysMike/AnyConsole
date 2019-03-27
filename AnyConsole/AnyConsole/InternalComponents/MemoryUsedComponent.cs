namespace AnyConsole.InternalComponents
{
    public class MemoryUsedComponent : BaseProcessComponent
    {
        private string _value;

        public MemoryUsedComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

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
            // CurrentProcess.WorkingSet64 is an expensive call so we will call it less frequently
            if (tickCount % 60 == 0)
            {
                var newValue = FormatSize(CurrentProcess.WorkingSet64);
                if (!newValue.Equals(_value))
                {
                    _value = newValue;
                    HasUpdates = false;
                }
            }
        }
    }
}
