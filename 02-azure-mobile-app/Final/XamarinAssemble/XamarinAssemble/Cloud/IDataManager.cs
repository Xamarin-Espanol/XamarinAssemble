using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinAssemble.Models;

namespace XamarinAssemble.Cloud
{
    public interface IDataManager
    {
        Task<IEnumerable<Session>> GetSessionsAsync();
        Task<IEnumerable<Speaker>> GetSpeakersAsync();
    }
}
