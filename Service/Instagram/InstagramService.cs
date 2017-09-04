using CsQuery;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using VkGroupManager.Service.JSON;
using VkGroupManager.Service.Telegram;
using VkGroupManager.JsonModel.Instagram;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace VkGroupManager.Service.Instagram
{
    public class InstagramService : IInstagramService
    {
        IJsonService _jsonService;
        ITelegramService _telegramService;

        public InstagramService()
        {
            _jsonService = new JsonService();
            _telegramService = new TelegramService();
        }

        /// <summary>
        /// Парсим страницу
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        async Task<string> IInstagramService.GetInstagramJsonAsync(string url)
        {
            string json = null;

            try
            {
                if (!url.Contains(@"instagram.com"))
                {
                    url = $@"https://www.instagram.com/{url}/?hl=ru";
                }

                var html = await DownloadPageAsync(url);
                CQ cq = CQ.Create(html);
                json = cq["body script"]
                    .FirstOrDefault(js => js.TextContent.Contains("_sharedData"))
                    .TextContent.Replace("window._sharedData =", string.Empty).TrimEnd(';');
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return json;
        }

        async Task<string> IInstagramService.GetNextJsonAsync(string query_id, string id, int first, string after)
        {
            string json = null;

            try
            {
                // https://www.instagram.com/graphql/query/?query_id=17888483320059182&variables={"id":"1187237132","first":100,"after":"AQAuVsBTXbqB0J2eDJholKSRf09-4yPON6WHTbM-0_UN9xSSEB67yHZwaCOPhQcTEYHXx6XmIljj3xUJaV-WnlV9EkFTzhXz0bBWCj2O45nMoA"}
                var query = JsonConvert.SerializeObject(new { id = id, first = first, after = after });

                var url = $@"https://www.instagram.com/graphql/query/?query_id={query_id}&variables={query}";
                json = await DownloadPageAsync(url);
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return json;
        }

        /// <summary>
        /// Грузим страницу
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> DownloadPageAsync(string url)
        {
            var result = string.Empty;

            try
            {
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
                        httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, sdch");
                        httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

                        using (HttpResponseMessage response = await httpClient.GetAsync(new Uri(url)))
                        {
                            var bytes = await response.Content.ReadAsByteArrayAsync();

                            Encoding encoding = Encoding.GetEncoding("utf-8");
                            result = encoding.GetString(bytes, 0, bytes.Length);
                        }

                        //using (HttpResponseMessage response = await httpClient.GetAsync(new Uri(url)))
                        //using (var responseStream = await response.Content.ReadAsStreamAsync())
                        //using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        //using (var streamReader = new StreamReader(decompressedStream))
                        //{
                        //    return await streamReader.ReadToEndAsync();
                        //}
                    }
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
