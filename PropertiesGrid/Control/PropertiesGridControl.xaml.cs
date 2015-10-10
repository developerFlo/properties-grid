using PropertiesGrid.Classes;
using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
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
        internal const double RowHeight = 30;

        PropertiesGridControlViewModel _viewModel;

        public PropertiesGridControl()
        {
            this.InitializeComponent();
            this._viewModel = new PropertiesGridControlViewModel();
            this.mainGrid.DataContext = this._viewModel;
            this.columnsControl.BaseControl = this;
            this.rowsControl.BaseControl = this;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await ReloadData();
        }

        private async Task ReloadData()
        {
            if (this.DataSource == null)
            {
                this._viewModel.Source = PGSourceWrapper.Default;
            }
            else
            {
                this._viewModel.Source = await PGSourceWrapper.CreateWrapperAsync(this.DataSource);

            }

            this.columnsControl.DataSourceChanged();
            this.rowsControl.DataSourceChanged();
        }

        #region Dependency Properties
        public static readonly DependencyProperty DataSourceProperty =
              DependencyProperty.RegisterAttached(
                  "DataSource", typeof(IPGSource), typeof(PropertiesGridControl),
                  new PropertyMetadata(null, DataSourceChanged));

        public static readonly DependencyProperty GroupTemplateProperty =
          DependencyProperty.Register(
              "GroupTemplate", typeof(DataTemplate), typeof(PropertiesGridControl));

        public static readonly DependencyProperty GroupPropertyTemplateProperty =
          DependencyProperty.Register(
              "GroupPropertyTemplate", typeof(DataTemplate), typeof(PropertiesGridControl));

        public static readonly DependencyProperty HeaderTemplateProperty =
          DependencyProperty.Register(
              "HeaderTemplate", typeof(DataTemplate), typeof(PropertiesGridControl));

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

        public DataTemplate GroupTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(GroupTemplateProperty);
            }
            set
            {
                this.SetValue(GroupTemplateProperty, value);
            }
        }

        public DataTemplate GroupPropertyTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(GroupPropertyTemplateProperty);
            }
            set
            {
                this.SetValue(GroupPropertyTemplateProperty, value);
            }
        }

        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(HeaderTemplateProperty);
            }
            set
            {
                this.SetValue(HeaderTemplateProperty, value);
            }
        }

        private static async void DataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertiesGridControl c = ((PropertiesGridControl)d);
            await c.ReloadData();
        }

        #endregion

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.columnsScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
            this.rowsScroll.ScrollToVerticalOffset(e.VerticalOffset);
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
