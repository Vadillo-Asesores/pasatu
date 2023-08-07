using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
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
        /// Returns the domain the computer is linked (and connected) to. Null if no domain can be found
        /// </summary>
        /// <returns></returns>
        public static String? GetDomainName()
        {
            try
            {
                return Domain.GetCurrentDomain().Name;
            }
            catch (ActiveDirectoryOperationException ex)
            {
                return null;
            }
        }

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
                if (controller.IPAddress != null)
                {
                    ipList.Add(IPAddress.Parse(controller.IPAddress));
                }
            }

            return ipList;

        }

        public static PasswordChangeResult ChangeUserADPassword(string dcIP, string domainName, string userName, string currentPassword, string newPassword)
        {
            try
            {
                string ldapPath = "LDAP://" + dcIP;
                DirectoryEntry directionEntry = new DirectoryEntry(ldapPath, domainName + "\\" + userName, currentPassword);
                if (directionEntry != null)

                {
                    DirectorySearcher search = new DirectorySearcher(directionEntry);
                    search.Filter = "(SAMAccountName=" + userName + ")";
                    SearchResult result = search.FindOne();
                    if (result != null)
                    {
                        DirectoryEntry userEntry = result.GetDirectoryEntry();
                        if (userEntry != null)
                        {
                            userEntry.Invoke("ChangePassword", new object[] { currentPassword, newPassword });
                            userEntry.CommitChanges();
                        }
                    }
                }

                return PasswordChangeResult.OK;
            }
            catch (Exception ex)
            {
                // Found that some exceptions come in the inner
                String Msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;

                if (Msg.Equals("El servidor no es funcional"))
                {
                    return new PasswordChangeResult(false, Msg, false);
                }
                else
                {
                    return new PasswordChangeResult(false, Msg);
                }
            }

        }
    }
}
