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
    public class RowViewModel : INotifyPropertyChanged, IPGUIElementViewModel
    {
        int _rowIndex;
        IPGRow _row;
        DataTemplate _headerTemplate;
        RowPropertyViewModel[] _properties;
        bool _isHovered;


        public RowViewModel(IPGRow row, DataTemplate headerTemplate, int rowIndex)
        {
            _row = row;
            _headerTemplate = headerTemplate;
            _rowIndex = rowIndex;
            _isHovered = false;
        }

        public int RowIndex { get { return _rowIndex; } }

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

        public RowPropertyViewModel[] Properties
        {
            get
            {
                return _properties;
            }

            set
            {
                if (_properties != value)
                {
                    _properties = value;
                    RaisePropertyChanged("Properties");
                }
            }
        }

        public IPGRow Row
        {
            get
            {
                return _row;
            }
            set
            {
                if (_row != value)
                {
                    _row = value;
                    RaisePropertyChanged("Row");
                }
            }
        }

        public DataTemplate HeaderTemplate
        {
            get
            {
                return _headerTemplate;
            }

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
            get { return _row; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
