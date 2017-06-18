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
        private int _noOfiterations;
        private double _error;
        private ObservableDataSource<Point> _d3DataSource;

        public double LearningRate { get { return _learningRate; } set { _learningRate = value; OnPropertyChanged("LearningRate"); } }
        public int Iterations { get { return _noOfiterations; } set { _noOfiterations = value; OnPropertyChanged("Iterations"); } }
        public double AnnError { get { return _error; } set { _error = value; OnPropertyChanged("AnnError"); } }
        public ObservableDataSource<Point> D3DataSource { get { return _d3DataSource; } set { _d3DataSource = value; OnPropertyChanged("D3DataSource"); } }

        public AnnTrainingModel()
        {
            _d3DataSource = new ObservableDataSource<Point>();
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
                        if (_noOfiterations < 0)
                            error = "Iterations must be a positove number.";
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
