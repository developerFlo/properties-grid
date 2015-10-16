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
    public class RowPropertyViewModel : INotifyPropertyChanged, IPGUIElementViewModel
    {
        int _rowIndex;
        int _propIndex;
        bool _isHovered;
        RowProperty _prop;
        DataTemplate _headerTemplate;

        public RowPropertyViewModel(RowProperty prop, DataTemplate headerTemplate, int rowIndex, int propIndex)
        {
            _prop = prop;
            _headerTemplate = headerTemplate;
            _rowIndex = rowIndex;
            _propIndex = propIndex;
            _isHovered = false;
        }

        public int RowIndex { get { return _rowIndex; } }
        public int PropIndex { get { return _propIndex; } }

        public bool IsHovered
        {
            get { return _isHovered; }
            set
            {
                if (_isHovered != value)
                {
                    _isHovered = value;
                    RaisePropertyChanged("IsHovered");
                }
            }
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

        public object DataItem
        {
            get { return _prop; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
