using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowNotifier.Shared.Tools
{
    public static class Helper
    {
        public static DateTime FromUnixEpochTime(long unixEpochTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixEpochTime);
        }
    }
}
