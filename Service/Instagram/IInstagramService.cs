using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkGroupManager.Service.Instagram
{
    public interface IInstagramService
    {
        Task<string> GetInstagramJsonAsync(string url);
        Task<string> GetNextJsonAsync(string query_id, string id, int first, string after);
    }
}