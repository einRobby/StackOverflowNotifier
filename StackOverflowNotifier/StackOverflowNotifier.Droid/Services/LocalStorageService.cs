using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using StackOverflowNotifier.Shared;
using StackOverflowNotifier.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(LocalStorageService))]
namespace StackOverflowNotifier.Droid
{
	public class LocalStorageService : ILocalStorageService
	{
		private readonly JsonSerializerSettings _JsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };

		public async Task SaveToFileAsync(string fileName, object content)
		{
			var json = JsonConvert.SerializeObject(content, Formatting.Indented, _JsonSerializerSettings);
			var filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
			using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
			{
				using (var stream = new StreamWriter(file))
				{
					await stream.WriteAsync(json);
				}
			}
		}

		public async Task<T> LoadFromFileAsync<T>(string fileName)
		{
			string json;
			var filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
			if (File.Exists(filePath))
			{
				using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read))
				{
					using (var stream = new StreamReader(file))
					{
						json = await stream.ReadToEndAsync();
					}
				}

				try
				{
					var content = JsonConvert.DeserializeObject<T>(json, _JsonSerializerSettings);
					return content;
				}
				catch (JsonException)
				{
					return default(T);
				}
			}

			return default(T);
		}
	}
}