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
    public class ColumnViewModel : INotifyPropertyChanged, IPGUIElementViewModel
    {
        int _columnIndex;
        bool _isHovered;
        IPGColumn _column;
        DataTemplate _headerTemplate;

        public ColumnViewModel(IPGColumn column, DataTemplate headerTemplate, int columnIndex)
        {
            _columnIndex = columnIndex;
            _column = column;
            _headerTemplate = headerTemplate;
            _isHovered = false;
        }

        public int ColumnIndex { get { return _columnIndex; } }

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
            get { return _column; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
