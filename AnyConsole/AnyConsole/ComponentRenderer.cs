using AnyConsole.InternalComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AnyConsole
{
    /// <summary>
    /// Built-in UI components rendering
    /// </summary>
    public class ComponentRenderer : IDisposable
    {
        private IDictionary<string, IComponent> _customComponents = new Dictionary<string, IComponent>();
        private IDictionary<Component, IComponent> _builtInComponents = new Dictionary<Component, IComponent>();
        private readonly IExtendedConsole _console;
        private bool _isDisposed;
        private ManualResetEvent _isRunning = new ManualResetEvent(false);
        private Thread _componentUpdateThread;
        private ConsoleDataContext _dataContext;

        public ComponentRenderer(IExtendedConsole console, ConsoleDataContext dataContext)
        {
            _console = console;
            _dataContext = dataContext;
            
            _isRunning.Reset();
            InitializeBuiltInComponents();
            _componentUpdateThread = new Thread(new ThreadStart(ComponentUpdateThread));
            _componentUpdateThread.Start();
        }

        private void InitializeBuiltInComponents()
        {
            _builtInComponents.Add(Component.DateTime, new DateTimeComponent(_dataContext));
            _builtInComponents.Add(Component.DateTimeUtc, new DateTimeUtcComponent(_dataContext));
            _builtInComponents.Add(Component.CurrentBufferLine, new LogBufferCurrentLineComponent(_dataContext));
            _builtInComponents.Add(Component.TotalLinesBuffered, new LogBufferTotalLinesComponent(_dataContext));
            _builtInComponents.Add(Component.ScrollbackPaused, new LogBufferIsPausedComponent(_dataContext));
            _builtInComponents.Add(Component.DiskUsed, new DiskUsedComponent(_dataContext));
            _builtInComponents.Add(Component.DiskFree, new DiskFreeComponent(_dataContext));
            _builtInComponents.Add(Component.MemoryFree, new MemoryFreeComponent(_dataContext));
            _builtInComponents.Add(Component.MemoryUsed, new MemoryUsedComponent(_dataContext));
            _builtInComponents.Add(Component.CpuUsage, new CpuUsageComponent(_dataContext));
            _builtInComponents.Add(Component.IP, new IPAddressComponent(_dataContext));
            _builtInComponents.Add(Component.LogSearch, new LogSearchComponent(_dataContext));
            foreach (var builtInComponent in _builtInComponents)
                builtInComponent.Value.Setup(_dataContext, builtInComponent.GetType().Name, _console);
        }


        private void ComponentUpdateThread()
        {
            var tickCount = 0UL;
            var customComponentWithoutThreadManagement = _customComponents.Where(x => x.Value.HasCustomThreadManagement).ToList();
            while (!_isRunning.WaitOne(100))
            {
                // todo: come up with an optimized way of only calling Tick on registered components
                foreach (var component in _builtInComponents)
                    component.Value.Tick(tickCount);
                foreach (var component in _customComponents)
                    component.Value.Tick(tickCount);
                tickCount++;
            }
        }

        public void RegisterComponent(string name, Type componentType)
        {
            if (!componentType.GetInterfaces().Contains(typeof(IComponent)))
                throw new ArgumentException(nameof(componentType), $"Type must implement IComponent");
            var component = Activator.CreateInstance(componentType) as IComponent;
            // call setup on the component
            component.Setup(_dataContext, name, _console);
            _customComponents.Add(name, component);
        }

        /// <summary>
        /// Render a built-in component
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public string Render(Component component)
        {
            if (component == Component.Custom)
                throw new ArgumentException($"Custom components must pass the component name!");
            return Render(component, null);
        }

        /// <summary>
        /// Render a built-in component
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public string Render(Component component, string componentName)
        {
            switch (component)
            {
                case Component.DateTime:
                    return _builtInComponents[component].Render($"YYYY-mm-dd hh:mm:ss tt");
                case Component.DateTimeUtc:
                    return _builtInComponents[component].Render($"YYYY-mm-dd hh:mm:ss tt");
                case Component.Date:
                    return _builtInComponents[component].Render($"YYYY-mm-dd");
                case Component.DateUtc:
                    return _builtInComponents[component].Render($"YYYY-mm-dd");
                case Component.Time:
                    return _builtInComponents[component].Render($"hh:mm:ss tt");
                case Component.TimeUtc:
                    return _builtInComponents[component].Render($"hh:mm:ss tt");
                case Component.Custom:
                    return Render(componentName, null);
                default:
                    return _builtInComponents[component].Render(null);
            }
            throw new ArgumentOutOfRangeException($"Unknown component: '{component}'");
        }

        /// <summary>
        /// Render a custom component
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public string Render(string componentName, object parameters)
        {
            if (!_customComponents.ContainsKey(componentName))
                throw new InvalidOperationException($"No component registered with name '{componentName}'");
            return _customComponents[componentName].Render(parameters);
        }

        /// <summary>
        /// Get if any components have updates to render
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public bool HasUpdates(IEnumerable<RowContent> componentsToRender)
        {
            var customComponents = componentsToRender
                .Where(x => x.Component == Component.Custom)
                .Select(x => x.ComponentName);
            var customComponentsHaveUpdates = _customComponents
                .Any(x => customComponents.Contains(x.Key) && x.Value.HasUpdates);
            var builtInComponents = componentsToRender
                .Where(x => _builtInComponents.Keys.Contains(x.Component))
                .Select(x => x.Component);
            var builtInComponentsHaveUpdates = _builtInComponents
                .Any(x => builtInComponents.Contains(x.Key) && x.Value.HasUpdates);

            return customComponentsHaveUpdates || builtInComponentsHaveUpdates;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            if (isDisposing)
            {
                _isRunning.Set();
                try
                {
                    if (!_componentUpdateThread.Join(500))
                        _componentUpdateThread.Abort();
                }
                catch (PlatformNotSupportedException)
                {
                    // ignore aborts on platforms that don't support it
                }
                foreach (var component in _customComponents)
                {
                    // dispose the component if it implements IDisposable
                    var type = component.Value.GetType();
                    if (type.GetInterfaces().Contains(typeof(IDisposable)))
                        ((IDisposable)component.Value).Dispose();
                }
            }
            _isDisposed = true;
        }

        #endregion
    }
}
