using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.WPF.Models
{
    public class LoginModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _userName;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

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
                    case "UserName":
                        if (string.IsNullOrEmpty(columnName))
                            error = "User name required.";
                        break;
                    case "Password":
                        {
                            if (string.IsNullOrEmpty(columnName))
                                error = "Password required.";
                            if (columnName.Length < 5)
                                error = "Minimul 5 characters required.";

                        break;
                        }
                }
                return (error);
            }
        }

        #endregion
    }
}
