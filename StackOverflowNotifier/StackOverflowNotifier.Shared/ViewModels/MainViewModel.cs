using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StackOverflowNotifier.Shared.Models;

namespace StackOverflowNotifier.Shared
{
	public class MainViewModel : ViewModelBase
	{
		private IUrlService _UrlService;
		private StackOverflowService _StackOverflowService;

		#region Properties

		private ObservableCollection<Question> _Questions;
		public ObservableCollection<Question> Questions
		{
			get { return _Questions; }
			set { _Questions = value; RaisePropertyChanged(); }
		}

		private ObservableCollection<string> _Tags;
		public ObservableCollection<string> Tags
		{
			get { return _Tags; }
			set { _Tags = value; RaisePropertyChanged(); }
		}

		private int _NewQuestionCount;
		public int NewQuestionCount
		{
			get { return _NewQuestionCount; }
			set { _NewQuestionCount = value; RaisePropertyChanged(); }
		}

		#endregion

		#region Commands

		private RelayCommand _RefreshCommand;
		public RelayCommand RefreshCommand
		{
			get
			{
				return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(async () =>
				{
					await RefreshAsync();
				}));
			}
		}

		private RelayCommand<Question> _OpenQuestionCommand;
		public RelayCommand<Question> OpenQuestionCommand
		{
			get
			{
				return _OpenQuestionCommand ?? (_OpenQuestionCommand = new RelayCommand<Question>((Question question) =>
				{
					_UrlService.OpenUrlInBrowser(question.Link);
				}));
			}
		}

		#endregion

		public MainViewModel(IUrlService urlService, StackOverflowService stackOverflowService)
		{
			_UrlService = urlService;
			_StackOverflowService = stackOverflowService;

			// Preset some tags
			Tags = new ObservableCollection<string>();
			Tags.Add("windows-10");
			Tags.Add("office365");
			Tags.Add("azure");

			Questions = new ObservableCollection<Question>();
			Questions.Add(new Question { Title = "Lorem ipsum dolor", Link = "http://www.google.de" });
			Questions.Add(new Question { Title = "Sit amet lorem ipsum dolor", Link = "http://www.google.de" });
		}

		public async Task RefreshAsync()
		{
			// Load questions for all tags
			var questionLists = new List<IEnumerable<Question>>();
			foreach (var tag in Tags)
			{
				var questionsForTag = await _StackOverflowService.GetUnansweredQuestionByTag(tag);
				questionLists.Add(questionsForTag);
			}

			// Merge and oder questions, remove dublicates
			var orderedQuestions = _StackOverflowService.MergeQuestions(questionLists);

			// Mark new questions
			//var oldQuestionsJson = await LocalStorage.LoadAsync("questions.json");
			//if (oldQuestionsJson != null)
			//{
			//	var oldQuestions = JsonConvert.DeserializeObject<ObservableCollection<Question>>(oldQuestionsJson);
			//	NewQuestionCount = StackOverflowConnector.MarkNewQuestions(orderedQuestions, oldQuestions);
			//}

			// Sync questions with ViewModel
			Questions = new ObservableCollection<Question>(orderedQuestions);

			// Save questions locally
			//if (saveNewQuestions)
			//{
			//	var newQuestionsJson = JsonConvert.SerializeObject(Questions);
			//	await LocalStorage.SaveAsync("questions.json", newQuestionsJson);
			//}
		}
	}
}