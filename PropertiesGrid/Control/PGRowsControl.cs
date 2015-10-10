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
        PropertiesGridControl _baseControl;

        public PGRowsControl() { }

        internal PropertiesGridControl BaseControl
        {
            get { return _baseControl; }
            set { _baseControl = value; }
        }

        internal void DataSourceChanged()
        {
            this.Children.Clear();
            if (_baseControl.DataSource != null && _baseControl.DataSource.Rows != null && _baseControl.DataSource.RowProperties != null)
            {
                IEnumerable<IPGRow> rows = _baseControl.DataSource.Rows;
                IEnumerable<IPGRowPropertyDef> props = _baseControl.DataSource.RowProperties;
                foreach (IPGRow grp in rows)
                {
                    //Reihenfolge ist entscheidend für die Arrange-Funktion
                    foreach (IPGRowPropertyDef prop in props)
                    {
                        FrameworkElement propElement = _baseControl.GroupPropertyTemplate.LoadContent() as FrameworkElement;
                        propElement.DataContext = new PGRowProperty(grp, prop);
                        this.Children.Add(propElement);
                    }

                    FrameworkElement grpElement = _baseControl.GroupTemplate.LoadContent() as FrameworkElement;
                    grpElement.DataContext = grp;
                    this.Children.Add(grpElement);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_baseControl != null && _baseControl.DataSource != null && _baseControl.DataSource.Rows != null && _baseControl.DataSource.RowProperties != null)
            {
                double width = availableSize.Width;
                if (double.IsPositiveInfinity(width))
                {
                    //Größe nicht vom übergeorndeten Element festgesetzt -> Größe auf Grund des Inhaltes ermitteln
                    double groupWidth = 0;
                    double propWidth = 0;

                    double availableGroupHeight = PropertiesGridControl.RowHeight * _baseControl.DataSource.RowProperties.Length;
                    foreach (FrameworkElement e in this.Children.OfType<FrameworkElement>())
                    {
                        if (e.DataContext is IPGRow)
                        {
                            if (!e.IsMeasureValid)
                                e.Measure(new Size(double.PositiveInfinity, availableGroupHeight));

                            groupWidth = Math.Max(groupWidth, e.DesiredSize.Width);
                        }
                        else if (e.DataContext is PGRowProperty)
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

                int rows = _baseControl.DataSource.Rows.Length;
                return new Size()
                {
                    Width = width,
                    Height = rows * PropertiesGridControl.RowHeight * _baseControl.DataSource.RowProperties.Length + 100 //+100 damit es sicher höher als die Scrollbar ist
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

            if (_baseControl != null && _baseControl.DataSource != null && _baseControl.DataSource.Rows != null && _baseControl.DataSource.RowProperties != null)
            {
                int rowIndex = 0;
                int rowPropIndex = 0;
                int rowPropertiesCount = _baseControl.DataSource.RowProperties.Length;
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
                    else if (e.DataContext is PGRowProperty)
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
