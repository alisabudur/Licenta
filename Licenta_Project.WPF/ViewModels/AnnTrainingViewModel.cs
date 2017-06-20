using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Licenta_Project.Common;
using Licenta_Project.DAL;
using Licenta_Project.WPF.Models;
using Licenta_Project.WPF.Services;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace Licenta_Project.WPF.ViewModels
{
    public class AnnTrainingViewModel : INotifyPropertyChanged
    {
        private bool _canExecute;

        private AnnTrainingModel _annTrainingModel;
        private DdsmService _ddsmService;
        private BackgroundWorker _worker;

        private ICommand _startTraining;
        private ICommand _refreshGraph;
        private ICommand _cancelCommand;

        public AnnTrainingViewModel()
        {
            _canExecute = true;
            _worker = new BackgroundWorker();
            _annTrainingModel = new AnnTrainingModel();

            var context = new DdsmContext();
            _ddsmService = new DdsmService
            (
                new BaseEntityRepository<DbCase>(context)
            );
        }

        public AnnTrainingModel AnnViewModel
        {
            get { return _annTrainingModel; }
            set { _annTrainingModel = value; OnPropertyChanged("AnnViewModel"); }
        }

        public ICommand StartTrainingCommand => _startTraining ?? (_startTraining = new CommandHandler(StartTraining, _canExecute));
        public ICommand RefreshGraphCommand => _refreshGraph ?? (_refreshGraph = new CommandHandler(RefreshGraph, _canExecute));
        public ICommand CancelCommad => _cancelCommand ?? (_cancelCommand = new CommandHandler(Cancel, _canExecute));

        private void StartTraining()
        {
            _worker.DoWork += worker_DoWork;
            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            var annInfo = new AnnInfo
            {
                LearningRate = _annTrainingModel.LearningRate,
                Epochs = _annTrainingModel.Epochs,
                Error = _annTrainingModel.AnnError
            };
            _worker.RunWorkerAsync(annInfo);
        }

        private void RefreshGraph()
        {
            _annTrainingModel.D3DataSourceError = new ObservableDataSource<Point>();
            _annTrainingModel.D3DataSourceAccuracy = new ObservableDataSource<Point>();
            _annTrainingModel.D3DataSourcePrecision = new ObservableDataSource<Point>();
            _annTrainingModel.D3DataSourceRecall = new ObservableDataSource<Point>();
            OnPropertyChanged("AnnViewModel");
        }

        private void Cancel()
        {
            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
                _annTrainingModel.D3DataSourceError = new ObservableDataSource<Point>();
                _annTrainingModel.D3DataSourceAccuracy = new ObservableDataSource<Point>();
                _annTrainingModel.D3DataSourcePrecision = new ObservableDataSource<Point>();
                _annTrainingModel.D3DataSourceRecall = new ObservableDataSource<Point>();
                OnPropertyChanged("AnnViewModel");
            }

        }

        #region Background worker

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var parameter = (AnnInfo)e.Argument;
            var annLearningRate = parameter.LearningRate;
            var annEpochs = parameter.Epochs;
            var annError = parameter.Error;

            var annService = new AnnService();

            var data = _ddsmService.GetTrainingData();
            var network = new ActivationNetwork(new SigmoidFunction(), 8, 10, 10, 10, 1);

            var learning = new BackPropagationLearning(network)
            {
                LearningRate = annLearningRate,
            };

            var needToStop = false;
            var epoch = 0;

            while (!needToStop && epoch < annEpochs)
            {
                var error = learning.RunEpoch(data.Input, data.Output) / data.Input.Length;
                annService.Network = network;
                var accuracy = annService.GetEpochAccuracy(data.Input, data.Output);
                var precision = annService.GetEpochPrecision(data.Input, data.Output);
                var recall = annService.GetEpochRecall(data.Input, data.Output);

                var errorInfo = new PerformanceInfo
                {
                    Epoch = epoch,
                    Error = error,
                    Accuracy = accuracy,
                    Precision = precision,
                    Recall = recall
                };

                worker.ReportProgress((epoch * 100) / annEpochs, errorInfo);
                if (error < annError)
                    needToStop = true;
                epoch++;
            }

            e.Result = network;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var performanceInfo = (PerformanceInfo)e.UserState;
            _annTrainingModel.D3DataSourceError.Collection.Add(new Point(performanceInfo.Epoch, performanceInfo.Error));
            _annTrainingModel.D3DataSourceAccuracy.Collection.Add(new Point(performanceInfo.Epoch, performanceInfo.Accuracy));
            _annTrainingModel.D3DataSourcePrecision.Collection.Add(new Point(performanceInfo.Epoch, performanceInfo.Precision));
            _annTrainingModel.D3DataSourceRecall.Collection.Add(new Point(performanceInfo.Epoch, performanceInfo.Recall));
            OnPropertyChanged("AnnViewModel");
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            worker.DoWork -= worker_DoWork;
            worker.ProgressChanged -= worker_ProgressChanged;
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            var network = (Network)e.Result;
            network.Save(Constants.NetworkFilePath);

            MessageBox.Show("Artificial neural network finished training!");
        }
        #endregion

        #region Property changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion

        #region Private classes

        private class AnnInfo
        {
            public double LearningRate { get; set; }
            public int Epochs { get; set; }
            public double Error { get; set; }
        }

        private class PerformanceInfo
        {
            public double Epoch { get; set; }
            public double Error { get; set; }
            public double Accuracy { get; set; }
            public double Precision { get; set; }
            public double Recall { get; set; }
        }

        #endregion
    }
}
