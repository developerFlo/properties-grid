﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Interfaces
{
    public interface IPGItem
    {
        IPGItem DeepCopy();
        void ResetValues(IPGItem item);
    }
}
