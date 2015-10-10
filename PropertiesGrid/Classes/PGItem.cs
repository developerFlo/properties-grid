using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Classes
{
    class PGItem
    {
        int _colIndex;
        int _rowIndex;
        int _propIndex;

        public PGItem(int colIndex, int rowIndex, int propIndex)
        {
            this._colIndex = colIndex;
            this._rowIndex = rowIndex;
            this._propIndex = propIndex;
        }

        public int ColIndex { get { return _colIndex; } }
        public int RowIndex { get { return _rowIndex; } }
        public int PropIndex { get { return _propIndex; } }
    }
}
