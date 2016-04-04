using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace StackOverflowNotifier
{
	public class NavigationService : INavigationService
	{
		public const string RootPageKey = "-- ROOT --";
		private const string ParameterKeyName = "ParameterKey";
		private Page _RootPage;

		private readonly Dictionary<string, Type> _PagesByKey = new Dictionary<string, Type>();

		private string _CurrentPageKey;
		public string CurrentPageKey
		{
			get
			{
				return _CurrentPageKey ?? RootPageKey;
			}
		}

		private Page _CurrentPage
		{
			get
			{
				if (!_PagesByKey.ContainsKey(CurrentPageKey))
				{
					throw new ArgumentException(string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", CurrentPageKey), "pageKey");
				}

				return (Page)Activator.CreateInstance(_PagesByKey[CurrentPageKey]);
			}
		}

		public NavigationService(Page rootPage)
		{
			_RootPage = rootPage;
		}

		public void Configure(string key, Type pageType)
		{
			lock (_PagesByKey)
			{
				if (_PagesByKey.ContainsKey(key))
				{
					_PagesByKey[key] = pageType;
				}
				else
				{
					_PagesByKey.Add(key, pageType);
				}
			}
		}

		public void GoBack()
		{
			_CurrentPage.Navigation.PopAsync();
		}

		public void NavigateTo(string pageKey)
		{
			NavigateTo(pageKey, null);
		}

		public async void NavigateTo(string pageKey, object parameter)
		{
			if (!_PagesByKey.ContainsKey(pageKey))
			{
				throw new ArgumentException(string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", pageKey), "pageKey");
			}


			Page pageToNativate;
			if (parameter == null)
				pageToNativate = (Page)Activator.CreateInstance(_PagesByKey[pageKey]);
			else
				pageToNativate = (Page)Activator.CreateInstance(_PagesByKey[pageKey], parameter);

			await _RootPage.Navigation.PushAsync(pageToNativate);
			_CurrentPageKey = pageKey;
				
		}
	}
}

