using PropertiesGrid.Classes;
using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PropertiesGrid.Control
{
    /// <summary>
    /// Interaktionslogik für PlanningGridControl.xaml
    /// </summary>
    public partial class PropertiesGridControl : UserControl
    {
        internal const double DataItemWidth = 40;
        internal const double RowHeight = 22;

        PropertiesGridControlViewModel _viewModel;

        public PropertiesGridControl()
        {
            this.InitializeComponent();
            this._viewModel = new PropertiesGridControlViewModel();
            this.mainGrid.DataContext = this._viewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadData();
        }

        private void ReloadData()
        {
            RowProperty[] visibleProperties = this.RowProperties.Where(p => p.IsVisible == true).ToArray();

            this._viewModel.Props = visibleProperties;
            this._viewModel.Source = (visibleProperties.Length == 0 || this.DataSource == null)?new PGSourceStub():this.DataSource;
            this._viewModel.RebaseOnSource(this.RowTemplate, this.PropertyTemplate, this.ColumnTemplate);

            this.itemTemplateSelector.Templates = visibleProperties.Select(r => r.ItemTemplate).ToArray();

            this.rowsControl.DataContext = this._viewModel;
            this.rowsControl.Refresh();

            this.columnsControl.DataContext = this._viewModel;
            this.columnsControl.Refresh();

            this._viewModel.SetUpToDate();
        }

        #region Dependency Properties
        public static readonly DependencyProperty DataSourceProperty =
              DependencyProperty.RegisterAttached(
                  "DataSource", typeof(IPGSource), typeof(PropertiesGridControl),
                  new PropertyMetadata(null, DataSourceChanged));

        public static readonly DependencyProperty RowPropertiesProperty =
            DependencyProperty.Register(
            "RowProperties", typeof(ObservableCollection<RowProperty>), typeof(PropertiesGridControl),
            new PropertyMetadata(new ObservableCollection<RowProperty>(), RowPropertiesChanged));

        public static readonly DependencyProperty RowTemplateProperty =
          DependencyProperty.Register(
              "RowTemplate", typeof(DataTemplate), typeof(PropertiesGridControl));

        public static readonly DependencyProperty PropertyTemplateProperty =
          DependencyProperty.Register(
              "PropertyTemplate", typeof(DataTemplate), typeof(PropertiesGridControl));

        public static readonly DependencyProperty ColumnTemplateProperty =
          DependencyProperty.Register(
              "ColumnTemplate", typeof(DataTemplate), typeof(PropertiesGridControl));

        public IPGSource DataSource
        {
            get
            {
                return (IPGSource)this.GetValue(DataSourceProperty);
            }
            set
            {
                this.SetValue(DataSourceProperty, value);
            }
        }

        public ObservableCollection<RowProperty> RowProperties
        {
            get { return (ObservableCollection<RowProperty>)this.GetValue(RowPropertiesProperty); }
            set { this.SetValue(RowPropertiesProperty, value); }
        }

        public DataTemplate RowTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(RowTemplateProperty);
            }
            set
            {
                this.SetValue(RowTemplateProperty, value);
            }
        }

        public DataTemplate PropertyTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(PropertyTemplateProperty);
            }
            set
            {
                this.SetValue(PropertyTemplateProperty, value);
            }
        }

        public DataTemplate ColumnTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ColumnTemplateProperty);
            }
            set
            {
                this.SetValue(ColumnTemplateProperty, value);
            }
        }
        private static void RowPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertiesGridControl c = ((PropertiesGridControl)d);
            c.ReloadData();
        }

        private static void DataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertiesGridControl c = ((PropertiesGridControl)d);
            c.ReloadData();
        }

        #endregion

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.Source is ScrollViewer)
            {
                this.columnsScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
                this.rowsScroll.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private void rowsScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void columnsScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}
