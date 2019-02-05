using System;

namespace AnyConsole.InternalComponents
{
    public class LogBufferCurrentLineComponent : BaseProcessComponent
    {
        private int _currentLogLine;

        public LogBufferCurrentLineComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
        }

        public override string Render(object parameters)
        {
            try
            {
                return $"Current: {_currentLogLine}";
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
            var currentLogLine = extendedConsole._bufferYCursor;
            if(_currentLogLine != currentLogLine)
            {
                _currentLogLine = currentLogLine;
                HasUpdates = true;
            }
        }
    }
}
