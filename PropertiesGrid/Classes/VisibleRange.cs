using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Classes
{
    struct VisibleRange
    {
        public int firstCol;
        public int firstRow;
        public int colCount;
        public int rowCount;

        public static VisibleRange Empty = new VisibleRange()
        {
            firstCol = 0,
            firstRow = 0,
            colCount = 0,
            rowCount = 0
        };
    }
}
