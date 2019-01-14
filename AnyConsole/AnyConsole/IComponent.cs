namespace AnyConsole
{
    /// <summary>
    /// Component interface
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// The string to render
        /// </summary>
        /// <returns></returns>
        string Render();

        /// <summary>
        /// True if the component has new updates to render
        /// </summary>
        bool HasUpdates { get; }
    }
}
