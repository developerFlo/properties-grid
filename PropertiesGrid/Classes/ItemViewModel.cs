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
    public class ItemViewModel : INotifyPropertyChanged, IPGUIElementViewModel
    {
        int _itemIndex;
        int _colIndex;
        int _rowIndex;
        int _propIndex;
        bool _inEditMode;
        bool _isHovered;
        IPGItem _item;
        IPGItem _resetItem;
        IPGColumn _column;
        PropertiesGridControlViewModel _vm;
        RowProperty _property;

        internal ItemViewModel(int colIndex, int rowIndex, int propIndex, int itemIndex, 
            IPGItem item, IPGColumn column, RowProperty property, PropertiesGridControlViewModel vm)
        {
            this._colIndex = colIndex;
            this._rowIndex = rowIndex;
            this._propIndex = propIndex;
            this._itemIndex = itemIndex;
            this._item = item;
            this._column = column;
            this._inEditMode = false;
            this._isHovered = false;
            this._vm = vm;
            this._property = property;
        }

        #region Properties

        public int ColIndex { get { return _colIndex; } }
        public int RowIndex { get { return _rowIndex; } }
        public int PropIndex { get { return _propIndex; } }
        public int ItemIndex { get { return _itemIndex; } }
        public IPGColumn Column { get { return _column; } }

        public IPGItem Item
        {
            get { return _item; }
            private set
            {
                if (_item != value)
                {
                    _item = value;
                    RaisePropertyChanged("Item");
                }
            }
        }

        public bool InEditMode
        {
            get { return _inEditMode; }
            internal set
            {
                if (_inEditMode != value)
                {
                    _inEditMode = value;
                    RaisePropertyChanged("InEditMode");

                    
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

        public object DataItem
        {
            get { return _item; }
        }
        
        internal RowProperty Property
        {
            get { return _property; }
            set { _property = value; }
        }

        #endregion

        public bool StartEdit()
        {
            if (_vm.Items.Any(i => i.InEditMode))
            {
                return false;
            }
            else
            {
                _resetItem = _item.DeepCopy();
                InEditMode = true;
                return true;
            }
        }

        public void StopEdit()
        {
            this.InEditMode = false;
            _resetItem = null;
        }

        public void Reset()
        {
            this.Item.ResetValues(_resetItem);
            StopEdit();
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}", _rowIndex, _propIndex, _colIndex);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
