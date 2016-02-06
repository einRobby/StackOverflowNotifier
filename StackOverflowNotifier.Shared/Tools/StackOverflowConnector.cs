using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackOverflowNotifier.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowNotifier.Shared.Tools
{
    public static class StackOverflowConnector
    {
        private static readonly string _BaseUrl = "https://api.stackexchange.com";
        private static HttpClient _HttpClient = new HttpClient();

        /// <summary>
        /// Loads a list of unanswered questions from Stack Overflow that matches the given tag
        /// </summary>
        /// <param name="tag">Stack Overflow tag</param>
        /// <param name="size">number of results</param>
        /// <returns>list of questions</returns>
        public static async Task<IEnumerable<Question>> GetUnansweredQuestionByTag(string tag, int size = 30)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            _HttpClient = new HttpClient(handler);

            var url = $"{_BaseUrl}/2.2/questions/unanswered?pagesize={size}&order=desc&sort=creation&tagged={tag}&site=stackoverflow";
            var respone = await _HttpClient.GetStringAsync(url);
            var json = JObject.Parse(respone);

            var questions = JsonConvert.DeserializeObject<IEnumerable<Question>>(json["items"].ToString());
            return questions;
        }

        /// <summary>
        /// Merges multiple lists of questions and orderes them descending by date
        /// </summary>
        /// <param name="questionLists">list of question lists</param>
        /// <returns>ordered and merged questions</returns>
        public static IEnumerable<Question> MergeQuestions(List<IEnumerable<Question>> questionLists)
        {
            if (!questionLists.Any())
                return new List<Question>();

            // Merge questions
            var resultQuestions = questionLists.First();
            for (var i = 1; i < questionLists.Count; i++)
            {
                resultQuestions = resultQuestions.Concat(questionLists[i]);
            }

            // Oder questions by date
            resultQuestions = resultQuestions.OrderByDescending(x => x.CreationDate);

            // Remove dublicates
            resultQuestions = resultQuestions.GroupBy(x => x.QuestionId).Select(y => y.First());

            return resultQuestions;
        }

        /// <summary>
        /// Compares a fresh list of questions with an old one and marks every question as 'new' that does not appear in the old list.
        /// </summary>
        /// <param name="newQuestions">list of recently loaded questions</param>
        /// <param name="oldQuestions">list of previously lodaded questions to compare</param>
        public static int MarkNewQuestions(IEnumerable<Question> newQuestions, IEnumerable<Question> oldQuestions)
        {
            var newQuestionCount = 0;

            // Previous expression was this
            //foreach (var question in newQuestions)
            //{
            //    var oldQuestion = oldQuestions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
            //    if (oldQuestion == null)
            //    {
            //        question.IsNew = true;
            //        newQuestionCount++;
            //    }
            //}

            // Resharper made this
            foreach (var question in from question in newQuestions
                                     let oldQuestion = oldQuestions.FirstOrDefault(q => q.QuestionId == question.QuestionId)
                                     where oldQuestion == null select question)
            {
                question.IsNew = true;
                newQuestionCount++;
            }

            return newQuestionCount;
        }
    }
}
