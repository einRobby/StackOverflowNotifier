using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackOverflowNotifier.Shared.Models;

namespace StackOverflowNotifier.Shared
{
	public class StackOverflowService
	{
		private readonly string _BaseUrl = "https://api.stackexchange.com";
		private readonly HttpService _HttpService;

		public StackOverflowService(HttpService httpService)
		{
			_HttpService = httpService;
		}

		/// <summary>
		/// Loads a list of unanswered questions from Stack Overflow that matches the given tag
		/// </summary>
		/// <param name="tag">Stack Overflow tag</param>
		/// <param name="size">number of results</param>
		/// <returns>list of questions</returns>
		public async Task<IEnumerable<Question>> GetUnansweredQuestionByTag(string tag, int size = 30)
		{
			var url = $"{_BaseUrl}/2.2/questions/unanswered?pagesize={size}&order=desc&sort=creation&tagged={WebUtility.UrlEncode(tag)}&site=stackoverflow";
			var response = await _HttpService.GetStringAsync(url);
			var json = JObject.Parse(response);

			var test = json["items"].ToString();

			var questions = JsonConvert.DeserializeObject<IEnumerable<Question>>(test);
			return questions;
		}

		/// <summary>
		/// Merges multiple lists of questions and orderes them descending by date
		/// </summary>
		/// <param name="questionLists">list of question lists</param>
		/// <returns>ordered and merged questions</returns>
		public IEnumerable<Question> MergeQuestions(List<IEnumerable<Question>> questionLists)
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
		public int MarkNewQuestions(IEnumerable<Question> newQuestions, IEnumerable<Question> oldQuestions)
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
									 where oldQuestion == null
									 select question)
			{
				question.IsNew = true;
				newQuestionCount++;
			}

			return newQuestionCount;
		}
	}
}

