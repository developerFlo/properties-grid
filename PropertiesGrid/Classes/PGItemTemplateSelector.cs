using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PropertiesGrid.Classes
{
    class PGItemTemplateSelector: DataTemplateSelector
    {
        public DataTemplate[] Templates { get; internal set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ItemViewModel ivm = (ItemViewModel)item;
            if (Templates != null && Templates.Length > ivm.PropIndex)
            {
                return Templates[ivm.PropIndex];
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
