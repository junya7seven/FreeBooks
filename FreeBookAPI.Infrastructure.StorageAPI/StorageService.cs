using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using FreeBookAPI.Application.DTO;
using FreeBookAPI.Application.Interfaces;
using static System.Net.WebRequestMethods;

namespace FreeBookAPI.Infrastructure.StorageAPI
{
    public class StorageService : IStorageService
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string BookPath;

        public StorageService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            BookPath = _configuration.GetValue<string?>("YandexDiskApi:BookPath");
            client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_configuration.GetValue<string>("YandexDiskApi:AccessToken")}");
            CheckFolders();
        }

        private async Task CheckFolders()
        {
            string url = $"https://cloud-api.yandex.net/v1/disk/resources?path=disk%3A%2FBooks";
            var response = await client.PutAsync(url, null);
            EnsureSuccessStatusCode.ProcessingStatusCode(response);
        }

        private async Task<string> GetUploadUrl(Guid id)
        {
            string url = $"https://cloud-api.yandex.net/v1/disk/resources/upload?path={BookPath}/{id}";

            var response = await client.GetAsync(url);
            EnsureSuccessStatusCode.ProcessingStatusCode(response);

            var responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody)["href"]?.ToString()
                ?? throw new Exception("Не удалось получить ссылку загрузки.");

        }
        private async Task<string> GetDownloadUrl(Guid id)
        {
            string url = $"https://cloud-api.yandex.net/v1/disk/resources/download?path={BookPath}/{id}";

            var response = await client.GetAsync(url);
            EnsureSuccessStatusCode.ProcessingStatusCode(response);

            var responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody)["href"]?.ToString()
                ?? throw new Exception("Не удалось получить ссылку загрузки.");

        }

        public async Task<BookFile> DownloadFile(Guid id)
        {
            var downloadUrl = await GetDownloadUrl(id);
            var response = await client.GetAsync(downloadUrl);
            EnsureSuccessStatusCode.ProcessingStatusCode(response);

            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            return new BookFile
            {
                Id = id,
                FileName = $"{id}.pdf",
                ContentType = "application/pdf",
                Content = fileBytes
            };
        }

        public async Task<bool> UploadFile(BookFile fileModel)
        {

            if (fileModel is null || fileModel.Content is null
                || fileModel.Content.Length == 0)
            {
                throw new ArgumentException("Файл не может быть пустым");
            }

            string uploadUrl = await GetUploadUrl(fileModel.Id);

            using var content = new ByteArrayContent(fileModel.Content);
            content.Headers.ContentType = new MediaTypeHeaderValue(fileModel.ContentType);

            var response = await client.PutAsync(uploadUrl, content);
            EnsureSuccessStatusCode.ProcessingStatusCode(response);
            return true;

        }

        public async Task<string?> GetImagePath(Guid id)
        {
            var url = $"https://cloud-api.yandex.net/v1/disk/resources?path={BookPath}/{id}";

            var response = await client.GetAsync(url);
            EnsureSuccessStatusCode.ProcessingStatusCode(response);
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            var sizes = json["sizes"]?.ToObject<JArray>();
            if (sizes is not null)
            {
                foreach (var size in sizes)
                {
                    if (size["name"]?.ToString() == "ORIGINAL")
                    {
                        return size["url"]?.ToString();
                    }
                }
            }
            return null;
        }

        public async Task DeleteFile(Guid id)
        {
            
            var url = $"https://cloud-api.yandex.net/v1/disk/resources?path={BookPath}/{id}";
            try
            {
                var response = await client.DeleteAsync(url);
                EnsureSuccessStatusCode.ProcessingStatusCode(response);
            }
            catch (FileNotFoundException)
            {
                // не обрабатывать!
            }
            
        }

    }
}
