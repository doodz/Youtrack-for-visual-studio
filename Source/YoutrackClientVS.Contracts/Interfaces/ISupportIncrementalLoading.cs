using System.Threading.Tasks;

namespace YouTrackClientVS.Contracts.Interfaces
{
    public interface ISupportIncrementalLoading
    {
        Task LoadNextPageAsync();
    }
}