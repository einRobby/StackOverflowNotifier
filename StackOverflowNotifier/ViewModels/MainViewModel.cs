using GalaSoft.MvvmLight;
using StackOverflowNotifier.Models;
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
        private ObservableCollection<Question> _Questions;
        public ObservableCollection<Question> Questions
        {
            get { return _Questions; }
            set { _Questions = value; RaisePropertyChanged(); }
        }
    }
}
