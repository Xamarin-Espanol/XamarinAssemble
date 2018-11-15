using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinAssemble.Models;

namespace XamarinAssemble.Cloud
{
    public interface IDataManager
    {
        Task<IEnumerable<Sessions>> GetSessionsAsync();
        Task<IEnumerable<Speakers>> GetSpeakersAsync();
    }
}
