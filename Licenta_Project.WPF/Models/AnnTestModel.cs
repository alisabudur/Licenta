using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.WPF.Models
{
    public class AnnTestModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _imagePath;
        private string _result;
        private int _patientAge;
        private int _density;

        public string ImagePath { get { return _imagePath; } set { _imagePath = value; OnPropertyChanged("ImagePath"); } }
        public string Result { get { return _result; } set { _result = value; OnPropertyChanged("Result"); } }
        public int PatientAge { get { return _patientAge; } set { _patientAge = value; OnPropertyChanged("PatientAge"); } }
        public int Density { get { return _density; } set { _density = value; OnPropertyChanged("Density"); } }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }

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
                    case "PatientAge":
                        if (_patientAge < 0)
                            error = "Patient age must be a positove number.";
                        break;
                    case "Density":
                        {
                            if (_density < 1 || _density > 4)
                                error = "Density must be a value between 1 - 4.";
                            break;
                        }
                }
                return (error);
            }
        }
        #endregion
    }
}
