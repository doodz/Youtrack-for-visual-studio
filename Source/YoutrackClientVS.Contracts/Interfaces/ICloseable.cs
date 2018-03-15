using System;

namespace YouTrackClientVS.Contracts.Interfaces
{
    public interface ICloseable
    {
        event EventHandler Closed;
    }
}