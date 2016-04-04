using System;
using StackOverflowNotifier.Shared;
using Xamarin.Forms;

namespace StackOverflowNotifier
{
	public class UrlService : IUrlService
	{
		public void OpenUrlInBrowser(string url)
		{
			Device.OpenUri(new Uri(url));
		}
	}
}