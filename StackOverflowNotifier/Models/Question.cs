using Newtonsoft.Json;
using StackOverflowNotifier.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowNotifier.Models
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

        public DateTime CreationDate
        {
            get
            {
                return Helper.FromUnixEpochTime(CreationDateEpoch);
            }
        }
    }
}
