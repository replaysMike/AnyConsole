using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AnyConsole.InternalComponents
{
    public class IPAddressComponent : BaseProcessComponent
    {
        private string _value;
        private ICollection<IPAddress> _ipAddressList;
        private int? _chosenIndex;
        private bool _renderCalled = false;

        public IPAddressComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
            UpdateIP();
        }

        private void UpdateIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            _ipAddressList = host.AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
        }

        public override string Render(object parameters)
        {
            if (parameters != null && _chosenIndex == null)
            {
                if (int.TryParse(parameters.ToString(), out int ipIndex))
                {
                    _chosenIndex = ipIndex;
                }
                else
                {
                    _chosenIndex = -1;
                }
            }

            try
            {
                return _value;
            }
            finally
            {
                HasUpdates = false;
                _renderCalled = true;
            }
        }

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            if (_renderCalled || tickCount % 20 == 0)
            {
                UpdateIP();
                string newValue;
                if (_chosenIndex == null || _chosenIndex < 0)
                {
                    newValue = $"{string.Join(", ", _ipAddressList)}";
                }
                else
                {
                    newValue = _ipAddressList.Skip(_chosenIndex.Value).FirstOrDefault().ToString();
                }
                if (!newValue.Equals(_value))
                {
                    _value = newValue;
                    HasUpdates = true;
                }
            }

        }
    }
}
