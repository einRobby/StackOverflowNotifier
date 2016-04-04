﻿using System;
namespace StackOverflowNotifier.Shared
{
	public class Owner
	{
		public int reputation { get; set; }
		public int user_id { get; set; }
		public string user_type { get; set; }
		public string profile_image { get; set; }
		public string display_name { get; set; }
		public string link { get; set; }
		public int? accept_rate { get; set; }
	}
}

