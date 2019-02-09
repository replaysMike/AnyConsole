using System;

namespace AnyConsole.InternalComponents
{
    public class DateTimeComponent : BaseProcessComponent
    {
        private string _value;
        private string _format;

        public DateTimeComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
        }

        public override string Render(object parameters)
        {
            _format = parameters?.ToString() ?? "yyyy-MM-dd hh:mm:ss tt";

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
            var newValue = $"{DateTime.Now.ToString(_format)}";
            if (!newValue.Equals(_value))
            {
                _value = newValue;
                HasUpdates = true;
            }
        }
    }
}
