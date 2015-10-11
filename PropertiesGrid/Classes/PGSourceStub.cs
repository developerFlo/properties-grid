using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Classes
{
    class PGSourceStub : IPGSource
    {
        IPGColumn[] _columns;
        IPGRow[] _rows;

        public PGSourceStub()
        {
            _columns = new IPGColumn[0];
            _rows = new IPGRow[0];
        }


        public IPGColumn[] Columns
        {
            get
            {
                return _columns;
            }
        }

        public IPGRow[] Rows
        {
            get
            {
                return _rows;
            }
        }
    }
}
