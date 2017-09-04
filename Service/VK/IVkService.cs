using System.Collections.Generic;
using System.Threading.Tasks;
using VkGroupManager.Models.VK;

namespace VkGroupManager.Service.VK
{
    public interface IVkService
    {
        Task<string> GetProfilesAsync(VkUser vkUser);
        Task<string> WallGetAsync(string access_token, string count, string offset, string owner_id, string domain, bool onlyGroup);
        Task<bool> IsAdmin(string access_token, string user_id, string checked_id);
        Task<string> GetByIdAsync(string access_token, params string[] group_ids);
        Task<string> WallPostAsync(string access_token, string group_id, string message, string publish_date_unixtime, string type, params string[] photo_url);
        Task<List<long>> GetTimePostAsync(string access_token, string count, string offset, string owner_id, string domain);
    }
}