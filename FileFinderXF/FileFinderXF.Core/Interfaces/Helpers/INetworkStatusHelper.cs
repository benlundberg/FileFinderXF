﻿namespace FileFinderXF.Core
{
    public interface INetworkStatusHelper
    {
        bool IsConnected { get; }
        bool HasWifi { get; }
        bool HasBluetooth { get; }
    }
}
