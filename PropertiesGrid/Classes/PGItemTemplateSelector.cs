using PropertiesGrid.Control;
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
        bool _onlyReturnItemTemplate;

        public PGItemTemplateSelector()
        {
            _onlyReturnItemTemplate = false;
        }

        public PGItemTemplateSelector(bool onlyReturnItemTemplate)
        {
            _onlyReturnItemTemplate = onlyReturnItemTemplate;
        }
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //Diese Methode wird 2 mal aufgerufen
            // 1.) Vom PGItemsControl: Hier soll falls definiert das ItemContainerTemplate verwendet werden
            // 2.) Vom ContentPresenter im ItemContainerTemplate: Hier soll das ItemTemplate (und später das EditTemplate) verwendet werden

            // Zu sehen ist dieser Unterschied im Parameter container.VisualParent auf den leider nur via Debugger zugegriffen werden kann


            ItemViewModel ivm = (ItemViewModel)item;
            DataTemplate template = null;

            //Standardmäßig falls vorhanden -> ItemContainerTemplate
            if (!_onlyReturnItemTemplate && ivm.Property.ItemContainerTemplate != null)
            {
                template = ivm.Property.ItemContainerTemplate;
            }
            //Falls keine ItemContainerTemplate vorhanden -> ItemTemplate
            if (template == null && ivm.Property.ItemTemplate != null)
            {
                template = ivm.Property.ItemTemplate;
            }
            //Falls kein ItemTemplate vorhanden -> Default Template
            if (template == null)
            {
                template = base.SelectTemplate(item, container);
            }

            return template;
        }
    }
}
