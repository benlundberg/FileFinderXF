using FileFinderXF.Core;
using System.Linq;
using Xamarin.Essentials;

namespace FileFinderXF
{
    public class NetworkStatusHelper : INetworkStatusHelper
    {
        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public bool HasWifi => Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi);

        public bool HasBluetooth => Connectivity.ConnectionProfiles.Contains(ConnectionProfile.Bluetooth);
    }
}
