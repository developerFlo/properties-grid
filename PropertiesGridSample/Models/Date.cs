using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PropertiesGridSample.Models
{
    class Date : IPGColumn
    {
        private DateTime _value;

        public Date(DateTime value)
        {
            this._value = value;
        }

        public string Name
        {
            get
            {
                return _value.ToString("d");
            }
        }

        public FrameworkElement UIElement
        {
            get; set;
        }
    }
}
