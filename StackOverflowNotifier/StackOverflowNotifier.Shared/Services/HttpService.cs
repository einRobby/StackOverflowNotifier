using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackOverflowNotifier.Shared
{
	public class HttpService
	{
		private HttpClient _HttpClient;

		public HttpService()
		{
			var handler = new HttpClientHandler();
			if (handler.SupportsAutomaticDecompression)
			{
				handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			}

			_HttpClient = new HttpClient(handler);
		}

		public async Task<string> GetStringAsync(string url)
		{
			return await _HttpClient.GetStringAsync(url);
		}

		public async Task<T> GetFromJsonAsync<T>(string url)
		{
			var response = await _HttpClient.GetStringAsync(url);
			var json = JObject.Parse(response);
			var result = JsonConvert.DeserializeObject<T>(json.ToString());

			return result;
		}
	}
}