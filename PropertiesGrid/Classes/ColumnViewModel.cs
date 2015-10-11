using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PropertiesGrid.Classes
{
    class ColumnViewModel : INotifyPropertyChanged
    {
        IPGColumn _column;
        DataTemplate _headerTemplate;

        public ColumnViewModel(IPGColumn column, DataTemplate headerTemplate)
        {
            _column = column;
            _headerTemplate = headerTemplate;
        }

        public IPGColumn Column
        {
            get { return _column; }
            set
            {
                if (_column != value)
                {
                    _column = value;
                    RaisePropertyChanged("Column");
                }
            }
        }

        public DataTemplate HeaderTemplate
        {
            get { return _headerTemplate; }
            set
            {
                if (_headerTemplate != value)
                {
                    _headerTemplate = value;
                    RaisePropertyChanged("HeaderTemplate");
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
