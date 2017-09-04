using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Service.Text
{
    public interface ITextService
    {
        Task<WallGet> SortedTextAsync(WallGet wallGetResponse, List<VkItemViewModel> oldItems);
        Task<string> TextFilterAsync(string text, Filter allFilter, Filter filter);
        Task<IEnumerable<VkItemViewModel>> SortingItemAsync(IEnumerable<VkItemViewModel> vkItemViewModel, Filter filterAll);
        string AddTag(string text, string tag);

        Task<string> RemoveTagAsync(string text);
        Task<string> RemoveAuthorAsync(string text);
        Task<string> RemoveVkLinkAsync(string text);
        Task<string> RemoveLinkAsync(string text);
    }
}