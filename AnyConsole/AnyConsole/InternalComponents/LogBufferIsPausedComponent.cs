namespace AnyConsole.InternalComponents
{
    public class LogBufferIsPausedComponent : BaseProcessComponent
    {
        private bool _isPaused;

        public LogBufferIsPausedComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
        }

        public override string Render(object parameters)
        {
            try
            {
                if(_isPaused)
                    return $"PAUSED";
                return string.Empty;
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
            var isPaused = currentLogLine > 0;

            if (_isPaused != isPaused)
            {
                _isPaused = isPaused;
                HasUpdates = true;
            }
        }
    }
}
