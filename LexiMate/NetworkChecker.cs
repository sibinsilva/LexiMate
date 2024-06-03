using Microsoft.Maui.Networking;

namespace LexiMate
{

    public class NetworkChecker : iNetworkChecker
    {
        public bool IsConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        public void CheckInternetConnection()
        {
            // currently just use the IsConnected property.
        }
    }

}
