using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AnyConsole.InternalComponents
{
    public class IPAddressComponent : BaseProcessComponent
    {
        private string _value;
        private ICollection<IPAddress> _ipAddress;

        public IPAddressComponent(ConsoleDataContext consoleDataContext) : base(consoleDataContext)
        {
            UpdateIP();
        }

        private void UpdateIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            _ipAddress = host.AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
        }

        public override string Render(object parameters)
        {
            try
            {
                return _value;
            }
            finally
            {
                HasUpdates = false;
            }
        }

        public override void Tick(ulong tickCount)
        {
            base.Tick(tickCount);
            if (tickCount == 0 || tickCount % 20 == 0)
            {
                UpdateIP();
                var newValue = $"{string.Join(", ", _ipAddress)}";
                if (!newValue.Equals(_value))
                {
                    _value = newValue;
                    HasUpdates = true;
                }
            }
            
        }
    }
}
