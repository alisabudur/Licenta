using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace Licenta_Project.WPF.Models
{
    public class AnnTrainingModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private double _learningRate;
        private int _epochs;
        private double _error;
        private ObservableDataSource<Point> _d3DataSourceError;
        private ObservableDataSource<Point> _d3DataSourceAccuracy;
        private ObservableDataSource<Point> _d3DataSourcePrecision;
        private ObservableDataSource<Point> _d3DataSourceRecall;

        public double LearningRate { get { return _learningRate; } set { _learningRate = value; OnPropertyChanged("LearningRate"); } }
        public int Epochs { get { return _epochs; } set { _epochs = value; OnPropertyChanged("Epochs"); } }
        public double AnnError { get { return _error; } set { _error = value; OnPropertyChanged("AnnError"); } }
        public ObservableDataSource<Point> D3DataSourceError { get { return _d3DataSourceError; } set { _d3DataSourceError = value; OnPropertyChanged("D3DataSourceError"); } }
        public ObservableDataSource<Point> D3DataSourceAccuracy { get { return _d3DataSourceAccuracy; } set { _d3DataSourceAccuracy = value; OnPropertyChanged("D3DataSourceAccuracy"); } }
        public ObservableDataSource<Point> D3DataSourcePrecision { get { return _d3DataSourcePrecision; } set { _d3DataSourcePrecision = value; OnPropertyChanged("D3DataSourcePrecision"); } }
        public ObservableDataSource<Point> D3DataSourceRecall { get { return _d3DataSourceRecall; } set { _d3DataSourceRecall = value; OnPropertyChanged("D3DataSourceRecall"); } }

        public AnnTrainingModel()
        {
            _d3DataSourceError = new ObservableDataSource<Point>();
            _d3DataSourceAccuracy = new ObservableDataSource<Point>();
            _d3DataSourcePrecision = new ObservableDataSource<Point>();
            _d3DataSourceRecall = new ObservableDataSource<Point>();
        }

        #region Property changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion

        #region IDataErrorInfo

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case "LearningRate":
                        if (_learningRate < 0)
                            error = "Learning rate must be a positove number.";
                        break;

                    case "Iterations":
                        if (Epochs < 0)
                            error = "Epochs must be a positove number.";
                        break;

                    case "AnnError":
                        if (_error < 0)
                            error = "Error must be a positove number.";
                        break;
                }
                return (error);
            }
        }
        #endregion
    }
}
