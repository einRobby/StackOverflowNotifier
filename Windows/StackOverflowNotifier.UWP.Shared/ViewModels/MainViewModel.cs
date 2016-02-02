using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using StackOverflowNotifier.Shared.Models;
using StackOverflowNotifier.Shared.Tools;
using StackOverflowNotifier.UWP.Shared.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowNotifier.UWP.Shared.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static MainViewModel _Current;
        public static MainViewModel Current
        {
            get { return _Current; }
            set { _Current = value; }
        }

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
            set { _Tags = value; RaisePropertyChanged();  }
        }

        private int _NewQuestionCount;
        public int NewQuestionCount
        {
            get { return _NewQuestionCount; }
            set { _NewQuestionCount = value; RaisePropertyChanged(); }
        }


        public MainViewModel()
        {
            _Current = this;

            Questions = new ObservableCollection<Question>();
            Tags = new ObservableCollection<string>();

            // Preset some tags
            Tags.Add("windows-10");
            Tags.Add("office365");
            Tags.Add("azure");

            if (IsInDesignModeStatic)
            {
                var demoQuestion = new Question() { Title = "Sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.", Tags = new List<string> { "windows-10", "office365", "uwp "} };
                Questions = new ObservableCollection<Question>();
                Questions.Add(demoQuestion);
                Questions.Add(demoQuestion);

                Tags = new ObservableCollection<string>();
            }
        }

        public async Task LoadQuestionsAsync(bool saveNewQuestions = true)
        {
            // Load questions for all tags
            var questionLists = new List<IEnumerable<Question>>();
            foreach (var tag in Tags)
            {
                var questionsForTag = await StackOverflowConnector.GetUnansweredQuestionByTag(tag);
                questionLists.Add(questionsForTag);
            }

            // Merge and oder questions, remove dublicates
            var orderedQuestions = StackOverflowConnector.MergeQuestions(questionLists);
            orderedQuestions = orderedQuestions.Distinct();

            // Mark new questions
            var oldQuestionsJson = await LocalStorage.LoadAsync("questions.json");
            if (oldQuestionsJson != null)
            {
                var oldQuestions = JsonConvert.DeserializeObject<ObservableCollection<Question>>(oldQuestionsJson);
                NewQuestionCount = StackOverflowConnector.MarkNewQuestions(orderedQuestions, oldQuestions);
            }

            // Sync questions with ViewModel
            Questions.Clear();
            Questions = new ObservableCollection<Question>(orderedQuestions);

            // Save questions locally
            if (saveNewQuestions)
            {
                var newQuestionsJson = JsonConvert.SerializeObject(Questions);
                await LocalStorage.SaveAsync("questions.json", newQuestionsJson);
            }
        }

        public async Task SaveTagsAsync()
        {
            var json = JsonConvert.SerializeObject(Tags);
            await LocalStorage.SaveAsync("tags.json", json);
        }

        public async Task LoadTagsAsync()
        {
            var json = await LocalStorage.LoadAsync("tags.json");
            if (json != null)
            {
                Tags = JsonConvert.DeserializeObject<ObservableCollection<string>>(json);
            }
        }
    }
}
