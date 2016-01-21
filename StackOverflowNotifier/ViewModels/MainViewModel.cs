using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using StackOverflowNotifier.Models;
using StackOverflowNotifier.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowNotifier.ViewModels
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

        public async Task SaveAsync()
        {
            var json = await JsonConvert.SerializeObjectAsync(Tags);
            await LocalStorage.SaveAsync("tags.json", json);
        }

        public async Task LoadAsync()
        {
            var json = await LocalStorage.LoadAsync("tags.json");
            if (json != null)
            {
                Tags = await JsonConvert.DeserializeObjectAsync<ObservableCollection<string>>(json);
            }            
        }
    }
}
