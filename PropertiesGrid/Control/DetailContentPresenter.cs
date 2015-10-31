using PropertiesGrid.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PropertiesGrid.Control
{
    public class DetailContentPresenter:ContentPresenter
    {
        public DetailContentPresenter()
        {
            this.ContentTemplateSelector = new PGItemTemplateSelector(true);
        }
    }
}
