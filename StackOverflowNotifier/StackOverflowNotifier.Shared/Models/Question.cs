using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackOverflowNotifier.Shared.Tools;
using System.Net;

namespace StackOverflowNotifier.Shared.Models
{
    public class Question
    {
        [JsonProperty(PropertyName = "question_id")]
        public int QuestionId { get; set; }
        [JsonProperty(PropertyName = "title")]
		public string Title { get; set; }
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }
        [JsonProperty(PropertyName = "creation_date")]
        public long CreationDateEpoch { get; set; }
        public bool IsNew { get; set; }

        public DateTime CreationDate
        {
            get
            {
                return Helper.FromUnixEpochTime(CreationDateEpoch).ToLocalTime();
            }
        }

		[JsonConstructor]
		public Question(int questionId, string title, string link, List<string> tags, long createDateEpoch)
		{
			this.QuestionId = questionId;
			this.Title = WebUtility.HtmlDecode(title);
			this.Link = link;
			this.Tags = tags;
			this.CreationDateEpoch = createDateEpoch;
		}
	}
}
