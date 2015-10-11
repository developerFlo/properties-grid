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
    class PGRowsControl:Panel
    {
        double _measuredGroupWidth = 0;
        double _measuredPropWidth = 0;

        public PGRowsControl() {}

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
                foreach (RowViewModel row in this.ViewModel.Rows)
                {
                    //Reihenfolge ist entscheidend für die Arrange-Funktion
                    foreach (RowPropertyViewModel prop in row.Properties)
                    {
                        FrameworkElement propElement = prop.HeaderTemplate.LoadContent() as FrameworkElement;
                        propElement.DataContext = prop.Prop;
                        this.Children.Add(propElement);
                    }

                    FrameworkElement grpElement = row.HeaderTemplate.LoadContent() as FrameworkElement;
                    grpElement.DataContext = row.Row;
                    this.Children.Add(grpElement);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.ViewModel != null)
            {
                double width = availableSize.Width;
                int rowPropertiesCount = this.ViewModel.Props.Count;

                if (double.IsPositiveInfinity(width))
                {
                    //Größe nicht vom übergeorndeten Element festgesetzt -> Größe auf Grund des Inhaltes ermitteln
                    double groupWidth = 0;
                    double propWidth = 0;

                    double availableGroupHeight = PropertiesGridControl.RowHeight * rowPropertiesCount;
                    foreach (FrameworkElement e in this.Children.OfType<FrameworkElement>())
                    {
                        if (e.DataContext is IPGRow)
                        {
                            if (!e.IsMeasureValid)
                                e.Measure(new Size(double.PositiveInfinity, availableGroupHeight));

                            groupWidth = Math.Max(groupWidth, e.DesiredSize.Width);
                        }
                        else if (e.DataContext is RowProperty)
                        {
                            if (!e.IsMeasureValid)
                                e.Measure(new Size(double.PositiveInfinity, PropertiesGridControl.RowHeight));

                            propWidth = Math.Max(groupWidth, e.DesiredSize.Width);
                        }
                    }

                    _measuredGroupWidth = groupWidth;
                    _measuredPropWidth = propWidth;
                    width = groupWidth + propWidth;
                }

                int rows = this.ViewModel.Rows.Length;
                return new Size()
                {
                    Width = width,
                    Height = rows * PropertiesGridControl.RowHeight * rowPropertiesCount + 100 //+100 damit es sicher höher als die Scrollbar ist
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
                int rowIndex = 0;
                int rowPropIndex = 0;
                int rowPropertiesCount = this.ViewModel.Props.Count;
                foreach (FrameworkElement e in this.Children.OfType<FrameworkElement>())
                {
                    if (e.DataContext is IPGRow)
                    {
                        e.Arrange(new Rect(
                            x: 0,
                            y: (rowIndex * rowPropertiesCount) * PropertiesGridControl.RowHeight,
                            width: _measuredGroupWidth,
                            height: PropertiesGridControl.RowHeight * rowPropertiesCount));

                        rowIndex++;
                        rowPropIndex = 0;
                    }
                    else if (e.DataContext is RowProperty)
                    {
                        e.Arrange(new Rect(
                            x: _measuredGroupWidth,
                            y: (rowIndex * rowPropertiesCount + rowPropIndex) * PropertiesGridControl.RowHeight,
                            width: _measuredPropWidth,
                            height: PropertiesGridControl.RowHeight));

                        rowPropIndex++;
                    }
                }
            }

            return finalSize;

        }
    }
}
