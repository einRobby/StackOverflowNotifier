using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackOverflowNotifier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace StackOverflowNotifier.Tools
{
    public class StackOverflowConnector
    {
        private readonly string _BaseUrl = "https://api.stackexchange.com";
        private HttpClient _HttpClient;

        public StackOverflowConnector()
        {
            _HttpClient = new HttpClient();
        }

        public async Task<List<Question>> GetUnansweredQuestionByTag(string tag)
        {
            var requestUri = new Uri(_BaseUrl + $"/2.2/questions/unanswered?pagesize=30&order=desc&sort=creation&tagged={tag}&site=stackoverflow");
            var response = await _HttpClient.GetStringAsync(requestUri);
            var json = JObject.Parse(response);

            var questions = JsonConvert.DeserializeObject<List<Question>>(json["items"].ToString());
            return questions;
        }
    }
}
