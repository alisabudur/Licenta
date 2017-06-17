using System;
using System.Collections.Generic;
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

        public AnnTrainingModel AnnViewModel
        {
            get { return _annTrainingModel; }
            set { _annTrainingModel = value; OnPropertyChanged("AnnViewModel"); }
        }

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

        public ICommand StartTrainingCommand => _startTraining ?? (_startTraining = new CommandHandler(StartTraining, _canExecute));

        private void StartTraining()
        {

            _worker.DoWork += worker_DoWork;
            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.WorkerReportsProgress = true;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            var annInfo = new AnnInfo
            {
                LearningRate = _annTrainingModel.LearningRate,
                Iterations = _annTrainingModel.Iterations,
                Error = _annTrainingModel.AnnError
            };
            _worker.RunWorkerAsync(annInfo);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker) sender;
            var parameter = (AnnInfo)e.Argument;
            var annLearningRate = parameter.LearningRate;
            var annIterations = parameter.Iterations;
            var annError = parameter.Error;

            var data = _ddsmService.GetTrainingData();
            var network = new ActivationNetwork(new SigmoidFunction(), 8, 10, 10, 10, 1);

            var learning = new BackPropagationLearning((ActivationNetwork)network)
            {
                LearningRate = annLearningRate
            };

            var needToStop = false;
            var iterations = 0;
            var iterationsArray = new double[annIterations];
            var errorArray = new double[annIterations];

            while (!needToStop && iterations < annIterations)
            {
                var error = learning.RunEpoch(data.Input, data.Output) / data.Input.Length;

                iterationsArray[iterations] = iterations;
                errorArray[iterations] = error;
                var errorInfo = new ErrorInfo { Iteration = iterationsArray, Error = errorArray };
                worker.ReportProgress((iterations * 100) / annIterations, errorInfo);
                if (error < annError)
                    needToStop = true;
                iterations++;
            }

            e.Result = network;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //var errorInfo = (ErrorInfo)e.UserState;
            //var xDataSource = new EnumerableDataSource<double>(errorInfo.Error);
            //var yDataSource = new EnumerableDataSource<double>(errorInfo.Iteration);
            //_annTrainingModel.D3DataSource = new CompositeDataSource(xDataSource, yDataSource);
            //_annTrainingModel.AnnError = e.ProgressPercentage;
            Application.Current.Dispatcher.Invoke(() => { _annTrainingModel.AnnError = e.ProgressPercentage; });
            //_annTrainingModel.AnnError = e.ProgressPercentage;
            OnPropertyChanged("AnnViewModel");

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = (BackgroundWorker) sender;
            worker.DoWork -= worker_DoWork;
            worker.ProgressChanged -= worker_ProgressChanged;
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            var network = (Network)e.Result;
            network.Save(Constants.NetworkFilePath);

            MessageBox.Show("Artificial neural network finished training!");
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

        private class AnnInfo
        {
            public double LearningRate { get; set; }
            public int Iterations { get; set; }
            public double Error { get; set; }
        }

        private class ErrorInfo
        {
            public double[] Iteration { get; set; }
            public double[] Error { get; set; }
        }
    }
}
