using System;

namespace AnyConsole.InternalComponents
{
    public class LogBufferTotalPagesComponent : BaseProcessComponent
    {
        private int _currentLogLine;
        private int _totalLogPages;
        private bool _isPaused;

        public LogBufferTotalPagesComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
            HasUpdates = true;
        }

        public override string Render(object parameters)
        {
            try
            {
                return $"{_totalLogPages}";
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
            if (extendedConsole != null && extendedConsole.Configuration != null
                && extendedConsole.Configuration.LogHistoryContainer != null
                && extendedConsole._fullLogHistory != null)
            {
                var itemsPerPage = (Console.WindowHeight - extendedConsole.Configuration.LogHistoryContainer.Index);
                var totalLogPages = extendedConsole._fullLogHistory.Count / itemsPerPage;

                if (_totalLogPages != totalLogPages)
                {
                    _totalLogPages = totalLogPages;
                    HasUpdates = true;
                }
            }
        }
    }
}
