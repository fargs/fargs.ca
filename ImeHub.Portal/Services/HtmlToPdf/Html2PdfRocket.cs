using ImeHub.Portal.Services.HtmlToPdf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.HtmlToPdf
{
    public class Html2PdfRocket : IHtmlToPdf
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly Html2PdfRocketOptions _options;
        private readonly ILogger<Html2PdfRocket> _logger;
        private readonly string _apiKey;

        public Html2PdfRocket(
            ILogger<Html2PdfRocket> logger,
            IHttpClientFactory httpClientFactory,
            IOptions<Html2PdfRocketOptions> options)
        {
            _options = options.Value;
            _logger = logger;
            _apiKey = _options.ApiKey;
            _httpClientFactory = httpClientFactory;

            _httpClient = _httpClientFactory.CreateClient();
            //_httpClient.BaseAddress = new Uri(_options.ApiUrl);
        }
        public async Task<byte[]> GenerateAsync(string content)
        {
            _logger.LogInformation($"{_httpClient.BaseAddress} / {_apiKey}");

            var httpContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("apikey", _apiKey),
                new KeyValuePair<string, string>("value", content),

                new KeyValuePair<string, string>("MarginTop", "10"),
                new KeyValuePair<string, string>("MarginBottom", "10"),
                new KeyValuePair<string, string>("MarginLeft", "10"),
                new KeyValuePair<string, string>("MarginRight", "10")
            });

            var result = await _httpClient.PostAsync(new Uri(_options.ApiUrl), httpContent);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            return await result.Content.ReadAsByteArrayAsync();
        }
    }

}
