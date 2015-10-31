using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PropertiesGrid.Control
{
    [ContentProperty("ItemTemplate")]
    public class RowProperty:DependencyObject
    {
        #region Dependency Properties
        public static readonly DependencyProperty ItemContainerTemplateProperty =
            DependencyProperty.Register(
            "ItemContainerTemplate", typeof(DataTemplate), typeof(RowProperty));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(RowProperty));

        public static readonly DependencyProperty EditTemplateProperty =
            DependencyProperty.Register(
            "EditTemplate", typeof(DataTemplate), typeof(RowProperty));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
            "Title", typeof(string), typeof(RowProperty));

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register(
            "IsVisible", typeof(bool), typeof(RowProperty),
            new PropertyMetadata(true));

        public DataTemplate ItemContainerTemplate
        {
            get { return (DataTemplate)this.GetValue(ItemContainerTemplateProperty); }
            set { this.SetValue(ItemContainerTemplateProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)this.GetValue(ItemTemplateProperty); }
            set { this.SetValue(ItemTemplateProperty, value); }
        }

        public DataTemplate EditTemplate
        {
            get { return (DataTemplate)this.GetValue(EditTemplateProperty); }
            set { this.SetValue(EditTemplateProperty, value); }
        }

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public bool IsVisible
        {
            get { return (bool)this.GetValue(IsVisibleProperty); }
            set { this.SetValue(IsVisibleProperty, value); }
        }
        #endregion
    }
}
