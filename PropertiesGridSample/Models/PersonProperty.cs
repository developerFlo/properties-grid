using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGridSample.Models
{
    class PersonProperty : IPGRowPropertyDef
    {
        public string Name
        {
            get; set;
        }
    }
}
