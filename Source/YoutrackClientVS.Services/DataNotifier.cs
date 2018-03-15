using System.ComponentModel.Composition;
using YouTrackClientVS.Contracts.Interfaces.Services;

namespace YouTrackClientVS.Services
{
    [Export(typeof(IDataNotifier))]
    [PartCreationPolicy(CreationPolicy.Shared)] //todo not very elegant solution to pass the information about state change. Make it better later
    public class DataNotifier : IDataNotifier
    {
        public bool ShouldUpdate { get; set; }
    }
}
