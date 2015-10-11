using PropertiesGrid.Classes;
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
        public PGColumnsControl() { }

        private PropertiesGridControlViewModel ViewModel
        {
            get
            {
                return this.DataContext as PropertiesGridControlViewModel;
            }
        }

        public void Refresh()
        {
            this.Children.Clear();
            if (this.ViewModel != null)
            {
                ColumnViewModel[] columns = this.ViewModel.Columns;
                foreach (ColumnViewModel col in columns)
                {
                    FrameworkElement obj = col.HeaderTemplate.LoadContent() as FrameworkElement;
                    obj.DataContext = col.Column;
                    this.Children.Add(obj);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.ViewModel != null)
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

                int dataColumns = this.ViewModel.Columns.Length;
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

            if (this.ViewModel != null)
            {
                int colIndex = 0;
                foreach (UIElement e in this.Children)
                {
                    e.Arrange(new Rect(
                        x: colIndex * PropertiesGridControl.DataItemWidth,
                        y: 0,
                        width: PropertiesGridControl.DataItemWidth,
                        height: finalSize.Height));
                    colIndex++;
                }
            }

            return finalSize;

        }
    }
}
