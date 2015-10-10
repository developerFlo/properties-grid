using PropertiesGridSample.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGridSample.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        PGSource _personData;

        public MainWindowViewModel()
        {
            _personData = new PGSource();
        }

        public PGSource PersonData
        {
            get { return _personData; }
            set
            {
                if(_personData != value)
                {
                    _personData = value;
                    RaisePropertyChanged("PersonData");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
