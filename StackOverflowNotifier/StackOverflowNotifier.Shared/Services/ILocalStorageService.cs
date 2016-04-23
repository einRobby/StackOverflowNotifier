using System;
using System.Threading.Tasks;

namespace StackOverflowNotifier.Shared
{
	public interface ILocalStorageService
	{
		Task SaveToFileAsync(string fileName, object content);
		Task<T> LoadFromFileAsync<T>(string fileName);
	}
}