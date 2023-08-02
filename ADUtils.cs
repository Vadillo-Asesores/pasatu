using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pasatu
{
    class ADUtils
    {
        public static DomainControllerCollection GetDCServers() => Domain.GetCurrentDomain().FindAllDiscoverableDomainControllers();

        /// <summary>
        /// Returns a list with all the AD Controllers IP with a non null IP.
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetDCServersIP()
        {
            DomainControllerCollection adCol = ADUtils.GetDCServers();
            List<IPAddress> ipList = new List<IPAddress>();

            foreach (DomainController controller in adCol)
            {
                if (controller.IPAddress != null) { 
                    ipList.Add(IPAddress.Parse(controller.IPAddress));
                }
            }

            return ipList;

        }
    }
}
