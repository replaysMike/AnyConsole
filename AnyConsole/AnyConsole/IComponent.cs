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
        string Render(object parameters);

        /// <summary>
        /// Handler is called periodically to do component logic updates.
        /// Called only when HasCustomThreadManagement=false
        /// </summary>
        void Tick(ulong tickCount);

        /// <summary>
        /// True if the component has new updates to render
        /// </summary>
        bool HasUpdates { get; }

        /// <summary>
        /// True if the component manages it's own thread. False to use
        /// the tick handler.
        /// </summary>
        bool HasCustomThreadManagement { get; }

        /// <summary>
        /// Called once upon initialization
        /// </summary>
        /// <param name="dataContext">A context you can use to transfer data</param>
        /// <param name="name">Name of the component</param>
        /// <param name="console">A reference to the console</param>
        void Setup(ConsoleDataContext dataContext, string name, IExtendedConsole console);
    }
}
