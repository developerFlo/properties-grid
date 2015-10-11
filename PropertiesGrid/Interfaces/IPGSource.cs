using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Interfaces
{
    public interface IPGSource
    {
        IPGRow[] Rows { get; }
        IPGColumn[] Columns { get; }
    }
}
