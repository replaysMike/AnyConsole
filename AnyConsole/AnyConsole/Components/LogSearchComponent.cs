namespace AnyConsole.Components
{
    public class LogSearchComponent : BaseProcessComponent
    {
        private string _value;

        public LogSearchComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext) { }

        public override string Render()
        {
            var isSearchEnabled = _consoleDataContext.GetData<bool>("IsSearchEnabled");
            if (isSearchEnabled)
            {
                return _value;
            }
            return string.Empty;
        }

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            var searchIndex = _consoleDataContext.GetData<int>("SearchIndex");
            var searchMatches = _consoleDataContext.GetData<int>("SearchMatches");
            var searchString = _consoleDataContext.GetData<string>("SearchString");
            _value = $"Search: \"{searchString}\"";
            if (searchMatches > 0)
                _value += $" Matches: {(searchIndex + 1)} of {searchMatches}";
            else if(!string.IsNullOrEmpty(searchString))
                _value += $" Matches: {searchMatches}";
        }
    }
}
