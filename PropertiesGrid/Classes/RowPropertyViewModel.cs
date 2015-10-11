using PropertiesGrid.Control;
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
    public class RowPropertyViewModel:INotifyPropertyChanged
    {
        RowProperty _prop;
        DataTemplate _headerTemplate;

        public RowPropertyViewModel(RowProperty prop, DataTemplate headerTemplate)
        {
            _prop = prop;
            _headerTemplate = headerTemplate;
        }
        
        public RowProperty Prop
        {
            get { return _prop; }
            set
            {
                if(_prop != value)
                {
                    _prop = value;
                    RaisePropertyChanged("Prop");
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
