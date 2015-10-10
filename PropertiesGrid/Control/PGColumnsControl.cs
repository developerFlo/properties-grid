using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PropertiesGrid.Control
{
    class PGColumnsControl:Panel
    {
        PropertiesGridControl _baseControl;

        public PGColumnsControl() { }

        internal PropertiesGridControl BaseControl
        {
            get { return _baseControl; }
            set { _baseControl = value; }
        }

        internal void DataSourceChanged()
        {
            this.Children.Clear();
            if (_baseControl.DataSource != null && _baseControl.DataSource.Columns != null)
            {
                IEnumerable<IPGColumn> columns = _baseControl.DataSource.Columns;
                foreach (IPGColumn col in columns)
                {
                    FrameworkElement obj = _baseControl.HeaderTemplate.LoadContent() as FrameworkElement;
                    obj.DataContext = col;
                    col.UIElement = obj;
                    this.Children.Add(obj);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_baseControl != null && _baseControl.DataSource != null && _baseControl.DataSource.Columns != null)
            {
                double height = availableSize.Height;
                if (double.IsPositiveInfinity(height))
                {
                    //Größe nicht vom übergeorndeten Element festgesetzt -> Größe auf Grund des Inhaltes ermitteln
                    height = 0;
                    foreach (UIElement e in this.Children)
                    {
                        if (!e.IsMeasureValid)
                            e.Measure(new Size(PropertiesGridControl.DataItemWidth, double.PositiveInfinity));
                        height = Math.Max(height, e.DesiredSize.Height);
                    }
                }

                int dataColumns = _baseControl.DataSource.Columns.Length;
                return new Size()
                {
                    Width = dataColumns * PropertiesGridControl.DataItemWidth + 100, //+100 damit es sicher breiter als die Scrollbar ist,
                    Height = height
                };
            }
            else
            {
                return Size.Empty;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect finalRect = new Rect(0, 0, finalSize.Width, finalSize.Height);

            if (_baseControl != null && _baseControl.DataSource != null && _baseControl.DataSource.Columns != null)
            {
                IEnumerable<IPGColumn> columns = _baseControl.DataSource.Columns;
                int colIndex = 0;
                foreach (IPGColumn col in columns)
                {
                    if (col.UIElement != null)
                    {
                        col.UIElement.Arrange(new Rect(
                            x: colIndex * PropertiesGridControl.DataItemWidth,
                            y: 0,
                            width: PropertiesGridControl.DataItemWidth,
                            height: finalSize.Height));
                    }
                    colIndex++;
                }
            }

            return finalSize;

        }
    }
}
