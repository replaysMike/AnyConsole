using System;
using System.Threading;

namespace AnyConsole.Runner
{
    public class RandomNumberComponent : IComponent, IDisposable
    {
        private bool _isDisposed;
        private ManualResetEvent _isRunning = new ManualResetEvent(false);
        private Thread _updateThread;
        private bool _hasUpdates;
        private int _randomNumber;
        private string _name;
        private IExtendedConsole _console;

        public bool HasUpdates { get { return _hasUpdates; } }

        public bool HasCustomThreadManagement => true;

        public RandomNumberComponent()
        {
        }

        public string Render()
        {
            _hasUpdates = false;
            return $"RND: {_randomNumber}";
        }

        /// <summary>
        /// Called once on startup
        /// </summary>
        /// <param name="name"></param>
        public void Setup(string name, IExtendedConsole console)
        {
            // we don't need these but we will store them
            _name = name;
            _console = console;

            _isRunning.Reset();
            _updateThread = new Thread(new ThreadStart(UpdateThread));
            _updateThread.IsBackground = true;
            _updateThread.Priority = ThreadPriority.BelowNormal;
            _updateThread.Start();
        }

        private void UpdateThread()
        {
            _randomNumber = GenerateRandomNumber();
            while (!_isRunning.WaitOne(2000))
            {
                _randomNumber = GenerateRandomNumber();
                _hasUpdates = true;
            }
        }

        private int GenerateRandomNumber()
        {
            return new Random().Next(1, 1000);
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
                _updateThread?.Join(500);
                _updateThread = null;
                _isRunning?.Dispose();
                _isRunning = null;
            }
            _isDisposed = true;
        }

        public void Tick(ulong tickCount)
        {
            // not used as we do our own thread management for updates
        }

        #endregion
    }
}
