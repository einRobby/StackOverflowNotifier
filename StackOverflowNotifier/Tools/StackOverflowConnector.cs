using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackOverflowNotifier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        /// <summary>
        /// Loads a list of unanswered questions from Stack Overflow that matches the given tag
        /// </summary>
        /// <param name="tag">Stack Overflow tag</param>
        /// <param name="size">number of results</param>
        /// <returns>list of questions</returns>
        public async Task<List<Question>> GetUnansweredQuestionByTag(string tag, int size = 30)
        {
            var requestUri = new Uri(_BaseUrl + $"/2.2/questions/unanswered?pagesize={size}&order=desc&sort=creation&tagged={tag}&site=stackoverflow");
            var response = await _HttpClient.GetStringAsync(requestUri);
            var json = JObject.Parse(response);

            var questions = JsonConvert.DeserializeObject<List<Question>>(json["items"].ToString());
            return questions;
        }

        /// <summary>
        /// Merges multiple lists of questions and orderes them descending by date
        /// </summary>
        /// <param name="questionLists">list of question lists</param>
        /// <returns>ordered and merged questions</returns>
        public List<Question> MergeQuestions(List<List<Question>> questionLists)
        {
            var mergedQuestions = new List<Question>();

            // Merge questions
            foreach (var questions in questionLists)
            {
                mergedQuestions.InsertRange(0, questions);
            }

            // Oder questions by date
            var orderedQuestions = mergedQuestions.OrderByDescending(x => x.CreationDate);

            return orderedQuestions.ToList();
        }

        /// <summary>
        /// Compares a fresh list of questions with an old one and marks every question as 'new' that does not appear in the old list.
        /// </summary>
        /// <param name="newQuestions">list of recently loaded questions</param>
        /// <param name="oldQuestions">list of previously lodaded questions to compare</param>
        public void MarkNewQuestions(List<Question> newQuestions, List<Question> oldQuestions)
        {
            foreach (var question in newQuestions)
            {
                var oldQuestion = oldQuestions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
                if (oldQuestion == null)
                {
                    question.IsNew = true;
                }
            }
        }
    }
}
