using System;
using System.Threading.Tasks;
using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IUserInformationService : IDisposable
    {
        ConnectionData ConnectionData { get; }
        YouTrackClientHistory ClientHistory { get; }
        Theme CurrentTheme { get; }
        void StartListening();
        Task Initialize();
    }
}