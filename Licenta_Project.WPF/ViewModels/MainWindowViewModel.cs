using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Licenta_Project.Common;
using Licenta_Project.WPF.Views;

namespace Licenta_Project.WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _canExecute;
        private Page _currentPage;
        private IDictionary<string, Page> _pages;
        private ICommand _navigateToAnnTrainingPage;
        private ICommand _navigateToAnnTestPage;


        public MainWindowViewModel()
        {
            _canExecute = true;
            _pages = new Dictionary<string, Page>()
            {
                { Constants.TrainingPage, new AnnTrainingPage()},
                { Constants.TestingPage, new AnnTestPage() }
            };
            Page = _pages[Constants.TrainingPage];
        }

        public Page Page
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("Page"); }
        }

        public ICommand NavigateToAnnTrainingPage => _navigateToAnnTrainingPage ?? (_navigateToAnnTrainingPage = new CommandHandler(
                                                         () => { Page = _pages[Constants.TrainingPage]; }, _canExecute));

        public ICommand NavigateToAnnTestingPage => _navigateToAnnTestPage ?? (_navigateToAnnTestPage = new CommandHandler(
                                                         () => { Page = _pages[Constants.TestingPage]; }, _canExecute));


        #region Property changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion
    }
}
