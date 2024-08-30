using System.Net.NetworkInformation;

namespace BudgetPlanner.States
{
    public class ApplicationState
    {
        public static bool HasInternetConnection { get; set; } = NetworkInterface.GetIsNetworkAvailable();
    }
}
