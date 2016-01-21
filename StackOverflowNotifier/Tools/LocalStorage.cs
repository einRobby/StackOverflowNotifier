using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace StackOverflowNotifier.Tools
{
    public static class LocalStorage
    {
        private static StorageFolder _Folder = ApplicationData.Current.RoamingFolder;

        public static async Task SaveAsync(string filename, string content)
        {
            var file = await _Folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
        }

        public static async Task<string> LoadAsync(string filename)
        {
            var allFiles = await _Folder.GetFilesAsync();
            var file = allFiles.FirstOrDefault(f => f.Name == filename);

            if (file != null)
            {
                return await FileIO.ReadTextAsync(file);
            }

            return null;
        }
    }
}
