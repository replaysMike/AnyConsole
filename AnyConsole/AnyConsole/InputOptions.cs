namespace AnyConsole
{
    /// <summary>
    /// Input handling options
    /// </summary>
    public enum InputOptions
    {
        /// <summary>
        /// Use built-in support for navigating the log and quitting the console
        /// </summary>
        UseBuiltInKeyOperations = 0,
        /// <summary>
        /// Use your own support and disable all built-in handling of input
        /// </summary>
        None = 1
    }
}
