using System;

namespace AnyConsole.InternalComponents
{
    public class LogBufferCurrentPageComponent : BaseProcessComponent
    {
        private int _currentLogPage;

        public LogBufferCurrentPageComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
            HasUpdates = true;
        }

        public override string Render(object parameters)
        {
            try
            {
                return $"{_currentLogPage}";
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
                && extendedConsole.Configuration.LogHistoryContainer != null && extendedConsole._fullLogHistory != null)
            {
                var itemsPerPage = (Console.WindowHeight - extendedConsole.Configuration.LogHistoryContainer.Index);
                var totalLogPages = extendedConsole._fullLogHistory.Count / itemsPerPage;
                var currentPage = (extendedConsole._bufferYCursor / itemsPerPage) + 1;
                if (currentPage < 0)
                    currentPage = 0;
                if (_currentLogPage != currentPage)
                {
                    _currentLogPage = currentPage;
                    HasUpdates = true;
                }
            }
        }
    }
}
