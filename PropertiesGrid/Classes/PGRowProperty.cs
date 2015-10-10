using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Classes
{
    public class PGRowProperty
    {
        IPGRow _row;
        IPGRowPropertyDef _prop;

        public PGRowProperty(IPGRow row, IPGRowPropertyDef prop){
            _row = row;
            _prop = prop;
        }

        public IPGRow Row
        {
            get { return _row; }
        }

        public IPGRowPropertyDef Prop
        {
            get { return _prop; }
        }
    }
}
