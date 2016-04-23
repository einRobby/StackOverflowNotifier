using System;
using PCLStorage;

namespace StackOverflowNotifier
{
	public interface IFileSystemProvider
	{
		IFileSystem GetPlatformFileSystem();
	}
}