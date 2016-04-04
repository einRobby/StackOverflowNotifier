using System;
using System.Collections.Generic;

namespace StackOverflowNotifier.Shared
{
	public class RootObject
	{
		public List<Item> items { get; set; }
		public bool has_more { get; set; }
		public int quota_max { get; set; }
		public int quota_remaining { get; set; }
	}
}

