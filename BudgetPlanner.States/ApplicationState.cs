using System.Net.NetworkInformation;

namespace BudgetPlanner.States
{
    public class ApplicationState
    {
        public static bool HasInternetConnection => CheckConnection();

        private static bool CheckConnection()
        {
            var all = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (var item in all)
            {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                if (item.Name.ToLower().Contains("virtual") || item.Description.ToLower().Contains("virtual"))
                    continue; //Exclude virtual networks set up by VMWare and others
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    var b = item.GetPhysicalAddress();
                    return true;
                }
            }

            return false;
        }
    }
}
