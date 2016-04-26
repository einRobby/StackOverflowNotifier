using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackOverflowNotifier.iOS;
using StackOverflowNotifier.Shared;

[assembly: Xamarin.Forms.Dependency(typeof(LocalStorageService))]
namespace StackOverflowNotifier.iOS
{
	public class LocalStorageService : ILocalStorageService
	{
		private readonly JsonSerializerSettings _JsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

		public async Task SaveToFileAsync(string fileName, object content)
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var json = JsonConvert.SerializeObject(content, Formatting.Indented, _JsonSerializerSettings);

			File.WriteAllText(Path.Combine(folder, fileName), json);
		}

		public async Task<T> LoadFromFileAsync<T>(string fileName)
		{
			try
			{
				var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				var json = File.ReadAllText(Path.Combine(folder, fileName));

				return JsonConvert.DeserializeObject<T>(json, _JsonSerializerSettings);
			}
			catch (FileNotFoundException)
			{
				return default(T);
			}
			catch (JsonException ex)
			{
				return default(T);
			}
		}
	}
}

