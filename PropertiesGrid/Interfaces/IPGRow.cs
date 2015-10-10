using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PropertiesGrid.Interfaces
{
    public interface IPGRow
    {
        string Name { get; }
        IEnumerable<IPGItem> Items { get; }
    }
}
