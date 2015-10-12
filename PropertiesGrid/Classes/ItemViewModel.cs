using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Classes
{
    public class ItemViewModel:INotifyPropertyChanged
    {
        int _colIndex;
        int _rowIndex;
        int _propIndex;
        bool _inEditMode;
        IPGItem _item;
        IPGColumn _column;

        public ItemViewModel(int colIndex, int rowIndex, int propIndex, IPGItem item, IPGColumn column)
        {
            this._colIndex = colIndex;
            this._rowIndex = rowIndex;
            this._propIndex = propIndex;
            this._item = item;
            this._column = column;
            this._inEditMode = false;
        }

        public int ColIndex { get { return _colIndex; } }
        public int RowIndex { get { return _rowIndex; } }
        public int PropIndex { get { return _propIndex; } }
        public IPGItem Item { get { return _item; } }
        public IPGColumn Column { get { return _column; } }

        public bool InEditMode
        {
            get { return _inEditMode; }
            set
            {
                if (_inEditMode != value)
                {
                    _inEditMode = value;
                    RaisePropertyChanged("InEditMode");
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
