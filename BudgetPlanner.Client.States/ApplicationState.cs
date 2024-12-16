using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace BudgetPlanner.Client.States
{
    public class ApplicationState
    {
        public static bool HasInternetConnection => CheckConnection();

        public static bool? IsDesktopBasedLifetime = null;

        private static bool CheckConnection()
        {

            var exclusionList = new List<string>()
            {
                // Windows 
                "virtual", 

                // Android
                "rmnet_data2",
                "epdg2",
                "dummy0"
            };

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var item in networkInterfaces)
            {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                var containsExclusion = false;
                foreach (var exclusion in exclusionList)
                {
                    if(!containsExclusion)
                        containsExclusion = item.Name.ToLower().Contains(exclusion.ToLower()) || item.Description.ToLower().Contains(exclusion.ToLower());
                }

                if (containsExclusion)
                    continue;


                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
