using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models;
using VkGroupManager.Models.VK;
using VkGroupManager.Service.Telegram;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Service.Text
{
    public class TextService : ITextService
    {
        ITelegramService _telegramService;

        public TextService()
        {
            _telegramService = new TelegramService();
        }

        /// <summary>
        /// Сравнение текстов
        /// </summary>
        /// <param name="wallGetResponse"></param>
        /// <param name="getKey"></param>
        async Task<WallGet> ITextService.SortedTextAsync(WallGet wallGetResponse, List<VkItemViewModel> oldItems)
        {
            try
            {
                if (wallGetResponse != null && oldItems != null)
                {
                    var itemCount = wallGetResponse.Items.Count;
                    List<Item> newItems = new List<Item>(itemCount);
                    for (int i = 0; i < itemCount; i++)
                    {
                        var item = wallGetResponse.Items[i];
                        var itemDate = item.Date;

                        var isContains = false;
                        for (int j = 0; j < oldItems.Count; j++)
                        {
                            var oldItemDate = oldItems[j].Date;

                            if (oldItemDate - 120 < itemDate && itemDate < oldItemDate + 120)
                            {
                                isContains = true;
                                break;
                            }
                        }

                        if (!isContains)
                        {
                            var itemKey = GetKey(item.Text ?? string.Empty);

                            if (itemKey.Length > 2)
                            {
                                for (int j = 0; j < oldItems.Count; j++)
                                {
                                    var oldKey = GetKey(oldItems[j].Text ?? string.Empty);
                                    var exNOCount = itemKey.Except(oldKey).Count();
                                    var exONCount = oldKey.Except(itemKey).Count();

                                    var newLength = itemKey.Length;
                                    var oldLength = oldKey.Length;

                                    double middle = 0d;
                                    if (newLength != 0 && oldLength != 0)
                                    {
                                        middle = (double)(exNOCount / newLength
                                           + exONCount / oldLength) / 2;
                                    }

                                    var add = false;

                                    if (itemKey.Length < 5)
                                    {
                                        add = oldItems[j].Text != item.Text;

                                        if (!add)
                                        {
                                            add = (1 - middle) < 0.95;
                                        }
                                    }
                                    else
                                    {
                                        add = (1 - middle) < 0.85;
                                    }

                                    if (add)
                                    {
                                        newItems.Add(item);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                newItems.Add(item);
                            }
                        }
                    }

                    wallGetResponse.Items = newItems;
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return wallGetResponse;
        }

        private string[] GetKey(string text)
         => Regex.Split(text, @"[.!?()\[\]{}\n]")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        /// <summary>
        /// Замены в тексте, в зависимости с фильтрами
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allFilter"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        async Task<string> ITextService.TextFilterAsync(string text, Filter allFilter, Filter filter)
        {
            if (text != null)
            {
                try
                {
                    if (allFilter?.RemoveText == true || filter?.RemoveText == true)
                    {
                        return string.Empty;
                    }

                    if (allFilter?.RemoveTag == true || filter?.RemoveTag == true)
                    {
                        text = await RemoveTagPrivate(text);
                    }

                    if (allFilter?.RemoveAuthor == true || filter?.RemoveAuthor == true)
                    {
                        text = await RemoveAuthorPrivate(text);
                    }

                    if (allFilter?.RemoveSmile == true || filter?.RemoveSmile == true)
                    {
                        text = await RemoveSmileAsync(text);
                    }

                    if (!string.IsNullOrEmpty(filter?.RepalaceFrom1))
                    {
                        text = text.Replace(filter.RepalaceFrom1, filter?.RepalaceTo1 ?? string.Empty);
                    }

                    if (!string.IsNullOrEmpty(filter?.RepalaceFrom2))
                    {
                        text = text.Replace(filter.RepalaceFrom2, filter?.RepalaceTo2 ?? string.Empty);
                    }

                    if (!string.IsNullOrEmpty(allFilter?.RepalaceFrom1))
                    {
                        text = text.Replace(allFilter.RepalaceFrom1, allFilter?.RepalaceTo1 ?? string.Empty);
                    }

                    if (!string.IsNullOrEmpty(allFilter?.RepalaceFrom2))
                    {
                        text = text.Replace(allFilter.RepalaceFrom2, allFilter?.RepalaceTo2 ?? string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    await _telegramService.SendMessageExceptionAsync(ex);
                }
            }

            return text?.Trim(' ', '\n');
        }

        /// <summary>
        /// Удаляем тэги из текста
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task<string> ITextService.RemoveTagAsync(string text)
        {
            return await RemoveTagPrivate(text);
        }

        private async Task<string> RemoveTagPrivate(string text)
        {
            if (text != null)
            {
                try
                {
                    string pattern = @"(?<hashtag>(#+\w+(@\w+)?))";
                    string replacePattern = string.Empty;

                    var regex = new Regex(pattern, RegexOptions.Compiled);
                    text = regex.Replace(text, replacePattern);
                }
                catch (Exception ex)
                {
                    await _telegramService.SendMessageExceptionAsync(ex);
                }
            }

            return text;
        }

        /// <summary>
        /// Удаляем строку с автором
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task<string> ITextService.RemoveAuthorAsync(string text)
        {
            return await RemoveAuthorPrivate(text);
        }

        /// <summary>
        /// Приватный метод удаления автора
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task<string> RemoveAuthorPrivate(string text)
        {
            if (text != null)
            {
                try
                {
                    string pattern = @"((Автор)\s?([^:^-]*[:-])([^\n]*[\n]?))|((Рецепт добавил)[:-][^\n]+)";
                    string replacePattern = string.Empty;

                    var regex = new Regex(pattern, RegexOptions.Compiled);
                    text = regex.Replace(text, replacePattern);
                }
                catch (Exception ex)
                {
                    await _telegramService.SendMessageExceptionAsync(ex);
                }
            }

            return text;
        }

        /// <summary>
        /// Удаляем wiki разметку
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task<string> ITextService.RemoveVkLinkAsync(string text)
        {
            if (text != null)
            {
                try
                {
                    string pattern = @"(?<link>([^\n]?\[\w+\|[^\]]*\](\n)?))";
                    string replacePattern = string.Empty;

                    var regex = new Regex(pattern, RegexOptions.Compiled);
                    text = regex.Replace(text, replacePattern);
                }
                catch (Exception ex)
                {
                    await _telegramService.SendMessageExceptionAsync(ex);
                }
            }

            return text;
        }

        /// <summary>
        /// Удаляем смайлы
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task<string> RemoveSmileAsync(string text)
        {
            if (text != null)
            {
                try
                {
                    var pattern = "\uD83D[\uDC00-\uDFFF]|\uD83C[\uDC00-\uDFFF]|\uFFFD";
                    string replacePattern = string.Empty;

                    var regex = new Regex(pattern, RegexOptions.Compiled);
                    text = regex.Replace(text, replacePattern);
                }
                catch (Exception ex)
                {
                    await _telegramService.SendMessageExceptionAsync(ex);
                }
            }

            return text;
        }

        /// <summary>
        /// Удаляем ссылки
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task<string> ITextService.RemoveLinkAsync(string text)
        {
            if (text != null)
            {
                try
                {
                    string pattern = @"(http(s)?:\/\/)?(www\.)?\w+\.(com|ru|\w+)\/?[A-z?=\-\&%0-9\+\/\~]*";
                    string replacePattern = string.Empty;

                    var regex = new Regex(pattern, RegexOptions.Compiled);
                    text = regex.Replace(text, replacePattern);
                }
                catch (Exception ex)
                {
                    await _telegramService.SendMessageExceptionAsync(ex);
                }
            }

            return text;
        }

        /// <summary>
        /// Добавляем тэги в текст
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        string ITextService.AddTag(string text, string tag) => $"{tag}\n{text}".Trim(' ', '\n');

        async Task<IEnumerable<VkItemViewModel>> ITextService.SortingItemAsync(IEnumerable<VkItemViewModel> vkItemViewModel, Filter filterAll)
        {
            try
            {
                var notGetWithPicture = filterAll?.GetWithPicture == true;
                var notCopyWithAuthor = filterAll?.CopyWithAuthor == true;
                var notGetWithLink = filterAll?.GetWithLink == true;
                var notGetWithVkLink = filterAll?.GetWithVkLink == true;
                var notGetWithWikiPage = filterAll?.GetWithWikiPage == true;

                vkItemViewModel = vkItemViewModel.Where(item =>
                {
                    var allFilter = item.FilterAll;
                    var filter = item.Filter;
                    var isWiew = true;

                    // Не копировать пасты без картинки
                    notGetWithPicture = notGetWithPicture || filter?.GetWithPicture == true;
                    // Не копировать посты с подписью (там где указан автор)
                    notCopyWithAuthor = notCopyWithAuthor || filter?.CopyWithAuthor == true;
                    // Не копировать посты со ссылкой
                    notGetWithLink = notGetWithLink || filter?.GetWithLink == true;
                    // Не копировать посты с ВК ссылками
                    notGetWithVkLink = notGetWithVkLink || filter?.GetWithVkLink == true;
                    // Не копировать посты с wiki страницами
                    notGetWithWikiPage = notGetWithWikiPage || filter?.GetWithWikiPage == true;

                    if (notGetWithPicture)
                    {
                        isWiew = NotGetWithPicture(item);
                        if (!isWiew)
                        {
                            return isWiew;
                        }
                    }

                    if (notCopyWithAuthor)
                    {
                        isWiew = NotCopyWithAuthor(item);
                        if (isWiew)
                        {
                            return !isWiew;
                        }
                    }

                    if (notGetWithLink)
                    {
                        isWiew = NotGetWithLink(item);
                        if (isWiew)
                        {
                            return !isWiew;
                        }
                    }

                    if (notGetWithVkLink)
                    {
                        isWiew = NotGetWithVkLink(item);
                        if (isWiew)
                        {
                            return !isWiew;
                        }
                    }

                    if (notGetWithWikiPage)
                    {
                        isWiew = NotGetWithWikiPage(item);
                        if (isWiew)
                        {
                            return !isWiew;
                        }
                    }

                    return true;
                });
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return vkItemViewModel;
        }

        private bool NotGetWithPicture
            (VkItemViewModel item)
        {
            var isView = false;
            if (!string.IsNullOrWhiteSpace(item?.GifPrew?.FirstOrDefault()?.Gif))
            {
                var previou = item?.GifPrew?.FirstOrDefault()?
                    .PreviewPhoto?.OrderByDescending(h => h.Height).FirstOrDefault()?.Src;
                var gif_src = item?.GifPrew?.FirstOrDefault()?.Gif;
                if (!string.IsNullOrWhiteSpace(previou) &&
                    !string.IsNullOrWhiteSpace(gif_src))
                {
                    isView = true;
                }
            }
            else
            {
                isView = item.Photo.Any(ph => !string.IsNullOrWhiteSpace(ph));
            }

            return isView;
        }

        bool NotCopyWithAuthor
            (VkItemViewModel item)
        {
            return item.Text.Contains("Автор") ||
                item.Text.Contains("Рецепт добавил");
        }

        private bool NotGetWithLink
            (VkItemViewModel item)
        {
            return
                item.Text.Contains("http") ||
                item.Text.Contains(".ru") ||
                item.Text.Contains(".com") ||
                item.Text.Contains("www.");
        }

        bool NotGetWithWikiPage
            (VkItemViewModel item)
        {
            return item.Text.Contains("[http");
        }

        bool NotGetWithVkLink
            (VkItemViewModel item)
        {
            return item.Text.Contains("[club");
        }
    }
}
