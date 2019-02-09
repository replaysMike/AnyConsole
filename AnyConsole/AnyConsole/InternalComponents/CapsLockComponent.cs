namespace AnyConsole.InternalComponents
{
    public class CapsLockComponent : BaseProcessComponent
    {
        private string _value;
        private string _format;

        public CapsLockComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
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

            var extendedConsole = _consoleDataContext.GetData<ExtendedConsole>("ExtendedConsole");
            if (extendedConsole != null)
            {
                var newValue = extendedConsole.CapsLockEnabled ? "CAPS" : "    ";
                if(!newValue.Equals(_value))
                {
                    _value = newValue;
                    HasUpdates = true;
                }
            }
        }
    }
}
