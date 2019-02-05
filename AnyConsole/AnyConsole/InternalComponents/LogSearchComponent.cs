namespace AnyConsole.InternalComponents
{
    public class LogSearchComponent : BaseProcessComponent
    {
        private string _value;

        public LogSearchComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

        public override string Render(object parameters)
        {
            try
            {
                var isSearchEnabled = _consoleDataContext.GetData<bool>("IsSearchEnabled");
                if (isSearchEnabled)
                {
                    return _value;
                }
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
            var searchIndex = _consoleDataContext.GetData<int>("SearchIndex");
            var searchMatches = _consoleDataContext.GetData<int>("SearchMatches");
            var searchString = _consoleDataContext.GetData<string>("SearchString");
            var newValue = $"Search: \"{searchString}\"";
            if (searchMatches > 0)
                newValue += $" Matches: {(searchIndex + 1)} of {searchMatches}";
            else if (!string.IsNullOrEmpty(searchString))
                newValue += $" Matches: {searchMatches}";

            if (!newValue.Equals(_value))
            {
                _value = newValue;
                HasUpdates = true;
            }
        }
    }
}
