using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace StackOverflowNotifier.Converters
{
	public class TagListToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is List<string>))
				return value;

			var str = "";
			foreach (var tag in (List<string>)value)
			{
				str += $"[{tag}] ";
			}

			return str;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}


