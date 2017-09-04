using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models.VK;
using VkGroupManager.Service.JSON;
using VkGroupManager.Service.Telegram;

namespace VkGroupManager.Service.VK
{
    public class VkService : IVkService
    {
        const string VERSION = "5.67";
        const int DELAY = 1000 / 3 + 9;

        IJsonService _jsonService;
        ITelegramService _telegramService;

        public VkService()
        {
            _jsonService = new JsonService();
            _telegramService = new TelegramService();
        }

        async Task<string> IVkService.GetProfilesAsync(VkUser vkUser)
        {
            var result = string.Empty;
            try
            {
                var url = @"https://api.vk.com/method/getProfiles";

                using (var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("access_token", vkUser.AccessToken),
                    new KeyValuePair<string, string>("uids", vkUser.UserVkId),
                    new KeyValuePair<string, string>("fields", @"photo_id, verified, sex, bdate, city, country, home_town, has_photo, photo_50, photo_100, photo_200_orig, photo_200, photo_400_orig, photo_max"),
                    new KeyValuePair<string, string>("v", VERSION),
                }))
                {
                    result = await HttpPostAsync(url, formContent);
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        /// <summary>
        /// Get запрос
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="result"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="owner_id"></param>
        /// <returns></returns>
        async Task<string> IVkService.WallGetAsync(string access_token, string count, string offset, string owner_id, string domain, bool onlyGroup)
        {
            string result = string.Empty;

            try
            {
                var url = "https://api.vk.com/method/wall.get";

                var values = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("access_token", access_token),
                        new KeyValuePair<string, string>("offset", offset),
                        new KeyValuePair<string, string>("count", count),
                        new KeyValuePair<string, string>("extended", "1"),
                        new KeyValuePair<string, string>("v", VERSION)
                    };

                if (onlyGroup)
                {
                    values.Add(new KeyValuePair<string, string>("filter", "owner"));
                }

                if (!string.IsNullOrWhiteSpace(owner_id.Trim('-')))
                {
                    values.Add(new KeyValuePair<string, string>("owner_id", owner_id));
                }
                else
                {
                    values.Add(new KeyValuePair<string, string>("domain", domain));
                }

                using (var formContent = new FormUrlEncodedContent(values))
                {
                    result = await HttpPostAsync(url, formContent);
                }

            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        async Task<string> IVkService.GetByIdAsync(string access_token, params string[] group_ids)
        {
            string result = string.Empty;

            try
            {
                var groouIds = string.Join(",", group_ids);
                var url = "https://api.vk.com/method/groups.getById";
                using (var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("access_token", access_token),
                    new KeyValuePair<string, string>("group_ids", groouIds),
                    new KeyValuePair<string, string>("v", VERSION)
                }))
                {
                    result = await HttpPostAsync(url, formContent);
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        /// <summary>
        /// Проверить, является-ли пользователь администратором группы
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="user_id"></param>
        /// <param name="checked_id">Id проверяемой группы</param>
        /// <returns></returns>
        async Task<bool> IVkService.IsAdmin(string access_token, string user_id, string checked_id)
        {
            var isAdmin = false;
            try
            {
                var url = "https://api.vk.com/method/groups.get";
                using (var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("access_token", access_token),
                    new KeyValuePair<string, string>("user_id", user_id),
                    new KeyValuePair<string, string>("filter", "admin"),
                    new KeyValuePair<string, string>("offset", "0"),
                    new KeyValuePair<string, string>("count", "1000"),
                    new KeyValuePair<string, string>("v", VERSION),
                }))
                {

                    var result = await HttpPostAsync(url, formContent);

                    if (result.Contains(checked_id))
                    {
                        isAdmin = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return isAdmin;
        }

        async Task<List<long>> IVkService.GetTimePostAsync(string access_token, string count, string offset, string owner_id, string domain)
        {
            var filter = "postponed";
            var listDate = new List<long>(100);

            try
            {
                var url = "https://api.vk.com/method/wall.get";

                var values = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("access_token", access_token),
                        new KeyValuePair<string, string>("offset", offset),
                        new KeyValuePair<string, string>("count", count),
                        new KeyValuePair<string, string>("filter", filter),
                        new KeyValuePair<string, string>("v", VERSION)
                    };

                if (!string.IsNullOrWhiteSpace(owner_id.Trim('-')))
                {
                    values.Add(new KeyValuePair<string, string>("owner_id", owner_id));
                }
                else
                {
                    values.Add(new KeyValuePair<string, string>("domain", domain));
                }

                using (var formContent = new FormUrlEncodedContent(values))
                {
                    var result = string.Empty;

                    try
                    {
                        using (var httpClient = new HttpClient())
                        using (HttpResponseMessage response = await httpClient.PostAsync(url, formContent))
                        using (HttpContent content = response.Content)
                        {
                            result = await content.ReadAsStringAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await _telegramService.SendMessageExceptionAsync(ex);
                    }

                    if (!result.Contains("error"))
                    {
                        var response = _jsonService.JsonConvertDeserializeObjectWithNull<WallGetResponse>(result);
                        foreach (var date in response.Response.Items.Select(it => it.Date))
                        {
                            if (date != 0)
                            {
                                listDate.Add(date);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return listDate;
        }

        /// <summary>
        /// Запостить сообщение
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="group_id"></param>
        /// <param name="message"></param>
        /// <param name="publish_date_unixtime"></param>
        /// <param name="photo_url"></param>
        /// <returns></returns>
        async Task<string> IVkService.WallPostAsync(string access_token, string group_id, string message, string publish_date_unixtime, string type, params string[] file_url)
        {
            string result = string.Empty;

            try
            {
                var url = "https://api.vk.com/method/wall.post";

                var attachments = new List<string>(10);
                if (file_url != null && file_url.All(o => !string.IsNullOrWhiteSpace(o)))
                {
                    foreach (var file in file_url)
                    {
                        if (string.IsNullOrWhiteSpace(type))
                        {
                            type = Path.GetExtension(file);
                        }

                        var saveWallfile = await SaveWallFileAsync(access_token, group_id, file, type);

                        attachments.Add(string.Join(",", JObject.Parse(saveWallfile)["response"]
                            .Select(o => $"{(type == "gif" ? "doc" : "photo")}{o["owner_id"]}_{o["id"]}")));
                    }
                }

                var content = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("access_token", access_token),
                    new KeyValuePair<string, string>("owner_id", $"-{group_id}"),
                    new KeyValuePair<string, string>("message", message),
                    new KeyValuePair<string, string>("publish_date", publish_date_unixtime),
                    new KeyValuePair<string, string>("v", VERSION)
                };


                if (attachments.Any(o => o != null))
                {
                    content.Add(new KeyValuePair<string, string>("attachments", string.Join(",", attachments)));
                }

                using (var formContent = new FormUrlEncodedContent(content))
                {
                    result = await HttpPostAsync(url, formContent);
                }

            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        private async Task<string> SaveWallFileAsync(string access_token, string group_id, string file_url, string file_type)
        {
            var result = string.Empty;
            try
            {
                var method = file_type.Contains("gif") ? "docs.getWallUploadServer" : "photos.getWallUploadServer";

                //upload_url, album_id, user_id
                var uploadServer = await FileGetWallUploadServerAsync(access_token, group_id, method);

                var upload_url = (string)JObject.Parse(uploadServer)["response"]?["upload_url"];
                var album_id = (string)JObject.Parse(uploadServer)["response"]?["album_id"];
                var user_id = (string)JObject.Parse(uploadServer)["response"]?["user_id"];

                if (!string.IsNullOrWhiteSpace(upload_url))
                {
                    var uploadedFiles = await UploadFilesAsync(access_token, file_url, upload_url, file_type);
                    //server, photo, hash
                    if (file_type != "gif")
                    {
                        var server = (string)JObject.Parse(uploadedFiles)["server"];
                        var photo = (string)JObject.Parse(uploadedFiles)["photo"];
                        var hash = (string)JObject.Parse(uploadedFiles)["hash"];

                        result = await SaveWallFileAsync(access_token, group_id,
                            photo, server, hash,
                            null);
                    }
                    else
                    {
                        var file = (string)JObject.Parse(uploadedFiles)["file"];

                        result = await SaveWallFileAsync(access_token, group_id,
                            null, null, null,
                            file);
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        private async Task<string> SaveWallFileAsync(string access_token, string group_id,
            string photo, string server, string hash,
            string file)
        {
            var result = string.Empty;

            try
            {
                var url = "https://api.vk.com/method/";

                var content = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("access_token", access_token),
                    new KeyValuePair<string, string>("group_id", group_id),
                    new KeyValuePair<string, string>("v", VERSION)
                };

                if (string.IsNullOrWhiteSpace(file))
                {
                    content.Add(new KeyValuePair<string, string>("hash", hash));
                    content.Add(new KeyValuePair<string, string>("server", server));
                    content.Add(new KeyValuePair<string, string>("photo", photo));

                    url += "photos.saveWallPhoto";
                }
                else
                {
                    content.Add(new KeyValuePair<string, string>("file", file));

                    url += "docs.save";
                }

                using (var formContent = new FormUrlEncodedContent(content))
                {
                    result = await HttpPostAsync(url, formContent);
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        /// <summary>
        /// Отправка файлов по ссылке
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="photo_url"></param>
        /// <param name="upload_url"></param>
        /// <returns></returns>
        private async Task<string> UploadFilesAsync(string access_token, string file_url, string upload_url, string file_type)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(file_url))
                {
                    Stopwatch delay = Stopwatch.StartNew();

                    using (HttpClientHandler clientHandler = new HttpClientHandler()
                    {
                        AllowAutoRedirect = true,
                        MaxAutomaticRedirections = 15,
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls
                    })
                    {
                        using (HttpClient httpClient = new HttpClient(clientHandler))
                        {
                            //httpClient.MaxResponseContentBufferSize = 2048;
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, sdch");
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

                            using (var responseContent = await httpClient.GetAsync(new Uri(file_url), HttpCompletionOption.ResponseHeadersRead))
                            {
                                if (responseContent.IsSuccessStatusCode)
                                {
                                    using (var stream = await responseContent.Content.ReadAsStreamAsync())
                                    //using (Stream stream = await httpClient.GetStreamAsync(new Uri(file_url)))
                                    using (var form = new MultipartFormDataContent
                                        {
                                            { new StringContent(access_token, Encoding.UTF8), "access_token" },
                                            { new StringContent(VERSION, Encoding.UTF8), "v" }
                                        })
                                    {
                                        var name = Path.GetFileName(file_url);
                                        using (var streamContent = CreateFileContent(stream, name, file_type))
                                        {
                                            form.Add(streamContent);

                                            using (HttpResponseMessage response = await httpClient.PostAsync(upload_url, form))
                                            using (HttpContent content = response.Content)
                                            {
                                                result = await content.ReadAsStringAsync();
                                            }
                                        }
                                    }
                                }
                            }
                            delay.Stop();

                            var delayTime = DELAY - delay.Elapsed.TotalMilliseconds;
                            if (delayTime > 0)
                            {
                                await Task.Delay((int)delayTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        /// <summary>
        /// Stream to StreamContent
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="name"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private StreamContent CreateFileContent(Stream stream, string name, string file_type)
        {
            var isPhoto = file_type != "gif";
            if (!isPhoto)
            {
                name = "isgif.gif";
            }

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = isPhoto ? "photo" : "file",
                FileName = name
            };

            var contentType = isPhoto ? "application/octet-stream" : "image/gif";
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }

        private async Task<string> FileGetWallUploadServerAsync(string access_token, string group_id, string method)
        {
            var result = string.Empty;
            try
            {
                var url = $"https://api.vk.com/method/{method}";
                using (var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("access_token", access_token),
                    new KeyValuePair<string, string>("group_id", group_id),
                    new KeyValuePair<string, string>("v", VERSION),
                }))
                {
                    result = await HttpPostAsync(url, formContent);
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }

        /// <summary>
        /// Пост запрос
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formContent"></param>
        /// <returns></returns>
        private async Task<string> HttpPostAsync(string url, FormUrlEncodedContent formContent)
        {
            var result = string.Empty;
            try
            {
                Stopwatch delay = Stopwatch.StartNew();

                using (HttpClientHandler clientHandler = new HttpClientHandler()
                {
                    AllowAutoRedirect = true,
                    MaxAutomaticRedirections = 15,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls
                })
                {
                    using (var httpClient = new HttpClient(clientHandler))
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, sdch");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

                        using (HttpResponseMessage response = await httpClient.PostAsync(url, formContent))
                        using (HttpContent content = response.Content)
                        {
                            result = await content.ReadAsStringAsync();
                        }
                    }
                }

                delay.Stop();

                var delayTime = DELAY - delay.Elapsed.TotalMilliseconds;
                if (delayTime > 0)
                {
                    await Task.Delay((int)delayTime);
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return result;
        }
    }
}
