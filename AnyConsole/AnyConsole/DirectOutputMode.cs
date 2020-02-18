namespace AnyConsole
{
    public enum DirectOutputMode
    {
        /// <summary>
        /// Static (always display) until explicitly cleared
        /// </summary>
        Static,
        /// <summary>
        /// Clears direct output when the log is modified
        /// </summary>
        ClearOnChange
    }
}
