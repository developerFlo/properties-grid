using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace PropertiesGrid.Control
{
    [ContentProperty("ItemTemplate")]
    public class RowProperty:DependencyObject
    {
        #region Dependency Properties
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(RowProperty));

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
            "Name", typeof(string), typeof(RowProperty));

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)this.GetValue(ItemTemplateProperty); }
            set { this.SetValue(ItemTemplateProperty, value); }
        }
        #endregion
    }
}
