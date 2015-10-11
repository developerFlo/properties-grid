using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Classes
{
    class ItemViewModel
    {
        int _colIndex;
        int _rowIndex;
        int _propIndex;
        IPGItem _item;

        public ItemViewModel(int colIndex, int rowIndex, int propIndex, IPGItem item)
        {
            this._colIndex = colIndex;
            this._rowIndex = rowIndex;
            this._propIndex = propIndex;
            this._item = item;
        }

        public int ColIndex { get { return _colIndex; } }
        public int RowIndex { get { return _rowIndex; } }
        public int PropIndex { get { return _propIndex; } }
        public IPGItem Item { get { return _item; } }
    }
}
