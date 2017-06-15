using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Licenta_Project.DAL;
using Licenta_Project.WPF.Models;
using Licenta_Project.WPF.Services;
using Microsoft.Win32;
using System.Drawing;
using AForge.Imaging;
using Licenta_Project.Common;

namespace Licenta_Project.WPF.ViewModels
{
    public class AnnTestViewModel : INotifyPropertyChanged
    {
        private bool _canExecute;

        private ICommand _uploadImage;
        private ICommand _testAnn;

        private AnnTestModel _annTestModel;
        private AnnService _annService;
        private DdsmService _ddsmService;

        public AnnTestViewModel()
        {
            _canExecute = true;
            _annTestModel = new AnnTestModel();
            _annService = new AnnService();
            _annService.LoadAnnFromFile(@"D:\Facultate\Licenta\Licenta\Licenta_Project\Resources\Network.txt");

            var context = new DdsmContext();
            _ddsmService = new DdsmService
            (
                new BaseEntityRepository<DbCase>(context)
            );
        }

        public AnnTestModel AnnViewModel
        {
            get { return _annTestModel; }
            set { _annTestModel = value; OnPropertyChanged("AnnViewModel"); }
        }

        public ICommand UploadImageCommand => _uploadImage ?? (_uploadImage = new CommandHandler(UploadImage, _canExecute));
        public ICommand TestAnnCommand => _testAnn ?? (_testAnn = new CommandHandler(TestAnn, _canExecute));

        public void UploadImage()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                _annTestModel.ImagePath = op.FileName;
                OnPropertyChanged("AnnViewModel");
            }
        }

        private void TestAnn()
        {
            var image = new Bitmap(_annTestModel.ImagePath);
            var histogram = new ImageStatistics(image).Red;

            var newDbCase = new DbCase
            {
                PatientAge = _annTestModel.PatientAge,
                Density = _annTestModel.Density,
                ImageMax = histogram.Max,
                ImageMin = histogram.Min,
                ImageMean = histogram.Mean,
                ImageStdDev = histogram.StdDev,
                ImageSkew = histogram.Skew(),
                ImageKurt = histogram.Kurt()
            };

            var normalizedInput = _ddsmService.NormalizeInputItem(newDbCase);
            _annTestModel.Result = _annService.Test(normalizedInput).ToString();
            OnPropertyChanged("AnnViewModel");
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
    }
}
