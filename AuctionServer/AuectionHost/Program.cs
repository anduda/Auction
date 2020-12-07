using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AuectionHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(AuctionServer.AuctionService)))
            {
                host.Open();
                Console.Write("Started");
                Console.ReadLine();
            }
        }
    }
}
