﻿using AnyConsole.Components;
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
        private IDictionary<Type, IComponent> _builtInComponents = new Dictionary<Type, IComponent>();
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
            _builtInComponents.Add(typeof(DiskUsedComponent), new DiskUsedComponent(_dataContext));
            _builtInComponents.Add(typeof(DiskFreeComponent), new DiskFreeComponent(_dataContext));
            _builtInComponents.Add(typeof(MemoryFreeComponent), new MemoryFreeComponent(_dataContext));
            _builtInComponents.Add(typeof(MemoryUsedComponent), new MemoryUsedComponent(_dataContext));
            _builtInComponents.Add(typeof(CpuUsageComponent), new CpuUsageComponent(_dataContext));
            _builtInComponents.Add(typeof(IPAddressComponent), new IPAddressComponent(_dataContext));
            _builtInComponents.Add(typeof(LogSearchComponent), new LogSearchComponent(_dataContext));
            foreach (var builtInComponent in _builtInComponents)
                builtInComponent.Value.Setup(_dataContext, builtInComponent.GetType().Name, _console);
        }


        private void ComponentUpdateThread()
        {
            var tickCount = 0UL;
            var customComponentWithoutThreadManagement = _customComponents.Where(x => x.Value.HasCustomThreadManagement).ToList();
            while (!_isRunning.WaitOne(100))
            {
                foreach (var component in _builtInComponents)
                    component.Value.Tick(tickCount);
                foreach (var component in _customComponents)
                    component.Value.Tick(tickCount);
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
                    return RenderDateTime();
                case Component.DateTimeUtc:
                    return RenderDateTimeUtc();
                case Component.Date:
                    return RenderDate();
                case Component.DateUtc:
                    return RenderDateUtc();
                case Component.Time:
                    return RenderTime();
                case Component.TimeUtc:
                    return RenderTimeUtc();
                case Component.CurrentBufferLine:
                    return RenderCurrentBufferLine();
                case Component.TotalLinesBuffered:
                    return RenderTotalLinesBuffered();
                case Component.ScrollbackPaused:
                    return RenderScrollbackPaused();
                case Component.LogSearch:
                    return _builtInComponents[typeof(LogSearchComponent)].Render();
                case Component.DiskUsed:
                    return _builtInComponents[typeof(DiskUsedComponent)].Render();
                case Component.DiskFree:
                    return _builtInComponents[typeof(DiskFreeComponent)].Render();
                case Component.MemoryUsed:
                    return _builtInComponents[typeof(MemoryUsedComponent)].Render();
                case Component.MemoryFree:
                    return _builtInComponents[typeof(MemoryFreeComponent)].Render();
                case Component.CpuUsage:
                    return _builtInComponents[typeof(CpuUsageComponent)].Render();
                case Component.IP:
                    return _builtInComponents[typeof(IPAddressComponent)].Render();
                case Component.Custom:
                    return Render(componentName);
            }
            throw new ArgumentOutOfRangeException($"Unknown component: '{component}'");
        }

        /// <summary>
        /// Render a custom component
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public string Render(string componentName)
        {
            if (!_customComponents.ContainsKey(componentName))
                throw new InvalidOperationException($"No component registered with name '{componentName}'");
            if(_customComponents[componentName].HasUpdates)
                return _customComponents[componentName].Render();
            return string.Empty;
        }

        #region Built-in Components

        private string RenderDateTime()
        {
            return $"{DateTime.Now}";
        }

        private string RenderDate()
        {
            return $"{DateTime.Now.ToShortDateString()}";
        }

        private string RenderTime()
        {
            return $"{DateTime.Now.ToString("hh:mm:ss tt")}";
        }

        private string RenderDateTimeUtc()
        {
            return $"{DateTime.UtcNow}";
        }

        private string RenderDateUtc()
        {
            return $"{DateTime.UtcNow.ToShortDateString()}";
        }

        private string RenderTimeUtc()
        {
            return $"{DateTime.UtcNow.ToString("hh:mm:ss tt")}";
        }

        private string RenderCurrentBufferLine()
        {
            var currentLogLine = ((ExtendedConsole)_console)._bufferYCursor;
            if (currentLogLine < 0)
                currentLogLine = 0;
            return $"Current: {currentLogLine}";
        }

        private string RenderTotalLinesBuffered()
        {
            var totalLines = ((ExtendedConsole)_console)._fullLogHistory.Count;
            return $"Total: {totalLines}";
        }

        private string RenderScrollbackPaused()
        {
            var isPaused = ((ExtendedConsole)_console)._bufferYCursor > 0;
            if (isPaused)
                return $"PAUSED";
            return string.Empty;
        }

        #endregion

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
