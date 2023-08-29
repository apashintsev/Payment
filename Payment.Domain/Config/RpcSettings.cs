using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Config
{
    public class RpcSettings
    {
        public List<NetworkSettings> NetworkSettings { get; set; }
    }

    public class NetworkSettings
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public int ChainId { get; set; }
        public string RpcUrl { get; set; }
    }
}
