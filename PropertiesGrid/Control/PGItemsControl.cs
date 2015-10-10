using PropertiesGrid.Classes;
using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace PropertiesGrid.Control
{
    class PGItemsControl:VirtualizingPanel,IScrollInfo
    {
        const int SCROLL_CREATE_ITEMS_DELAY_MS = 100;
        const int CREATE_MARGIN_ELEMENTS = 2;

        VisibleRange _prevVisibleRange = VisibleRange.Empty;
        VisibleRange _calculatedVisibleRange = VisibleRange.Empty;

        public PGItemsControl()
        {
            this.RenderTransform = _trans;
        }

        #region Dependency Properties
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.RegisterAttached(
                "DataSource", typeof(PGSourceWrapper), typeof(PGItemsControl),
                new PropertyMetadata(PGSourceWrapper.Default));

        public PGSourceWrapper DataSource
        {
            get 
            {
                return (PGSourceWrapper)this.GetValue(DataSourceProperty);
            }
            set
            {
                this.SetValue(DataSourceProperty, value);
            }
        }
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateScrollInfo(availableSize);

            // Figure out range that's visible based on layout algorithm
            VisibleRange visibleRange = GetVisibleRange();
            _prevVisibleRange = visibleRange;
            Task.Delay(SCROLL_CREATE_ITEMS_DELAY_MS).ContinueWith((t) =>
            {
                //Wartet für die angegeben Zeitspanne
                //Falls sich der angezeigte Bereich in dieser Zeit nicht geändert hat
                // -> Child-Element erzeugen
                if(visibleRange.Equals(_prevVisibleRange) && !visibleRange.Equals(_calculatedVisibleRange))
                {
                    _calculatedVisibleRange = visibleRange;
                    CalculateChildren(visibleRange);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
            return availableSize;
        }

        private void CalculateChildren(VisibleRange visibleRange)
        {
            // We need to access InternalChildren before the generator to work around a bug
            UIElementCollection children = this.InternalChildren;
            IItemContainerGenerator generator = this.ItemContainerGenerator;

            int columCount = DataSource.Columns.Length;

            int lastRow = visibleRange.firstRow + visibleRange.rowCount;
            for (int r = visibleRange.firstRow; r < lastRow; r++)
            {
                int firstVisibleItemInRow = r * columCount + visibleRange.firstCol;
                int lastVisibleItemInRow = firstVisibleItemInRow + visibleRange.colCount;
                GeneratorPosition startPos = generator.GeneratorPositionFromIndex(firstVisibleItemInRow);

                // Get index where we'd insert the child for this position. If the item is realized
                // (position.Offset == 0), it's just position.Index, otherwise we have to add one to
                // insert after the corresponding child
                int childIndex = (startPos.Offset == 0) ? startPos.Index : startPos.Index + 1;

                using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
                {
                    for (int itemIndex = firstVisibleItemInRow; itemIndex <= lastVisibleItemInRow; ++itemIndex, ++childIndex)
                    {
                        bool newlyRealized;

                        // Get or create the child
                        UIElement child = generator.GenerateNext(out newlyRealized) as UIElement;
                        if (newlyRealized)
                        {
                            // Figure out if we need to insert the child at the end or somewhere in the middle
                            if (childIndex >= children.Count)
                            {
                                base.AddInternalChild(child);
                            }
                            else
                            {
                                base.InsertInternalChild(childIndex, child);
                            }
                            generator.PrepareItemContainer(child);
                        }
                        else
                        {
                            // The child has already been created, let's be sure it's in the right spot
                            Debug.Assert(child == children[childIndex], "Wrong child was generated");
                        }

                        // Measurements will depend on layout algorithm
                        child.Measure(GetChildSize());
                    }
                }
            }

            // Note: this could be deferred to idle time for efficiency
            CleanUpItems(visibleRange, columCount);
        }

        /// <summary>
        /// Arrange the children
        /// </summary>
        /// <param name="finalSize">Size available</param>
        /// <returns>Size used</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            IItemContainerGenerator generator = this.ItemContainerGenerator;

            UpdateScrollInfo(finalSize);

            for (int i = 0; i < this.Children.Count; i++)
            {
                UIElement child = this.Children[i];

                // Map the child offset to an item offset
                int itemIndex = generator.IndexFromGeneratorPosition(new GeneratorPosition(i, 0));

                ArrangeChild(itemIndex, child, finalSize);
            }

            return finalSize;
        }

        /// <summary>
        /// Position a child
        /// </summary>
        /// <param name="itemIndex">The data item index of the child</param>
        /// <param name="child">The element to position</param>
        /// <param name="finalSize">The size of the panel</param>
        private void ArrangeChild(int itemIndex, UIElement child, Size finalSize)
        {
            int columns = DataSource.Columns.Length;

            int row = itemIndex / columns;
            int column = itemIndex % columns;

            child.Arrange(new Rect(column * PropertiesGridControl.DataItemWidth, row * PropertiesGridControl.RowHeight, PropertiesGridControl.DataItemWidth, PropertiesGridControl.RowHeight));
        }


        /// <summary>
        /// Revirtualize items that are no longer visible
        /// </summary>
        private void CleanUpItems(VisibleRange visibleRange, int columCount)
        {
            UIElementCollection children = this.InternalChildren;
            if (children.Count > 0)
            {
                IItemContainerGenerator generator = this.ItemContainerGenerator;

                int firstVisibleIndex = visibleRange.firstRow * columCount + visibleRange.firstCol;
                int lastVisibleIndex = (visibleRange.firstRow + visibleRange.rowCount) * columCount + visibleRange.firstCol + visibleRange.colCount;

                for (int i = children.Count - 1; i >= 0; i--)
                {
                    GeneratorPosition childGeneratorPos = new GeneratorPosition(i, 0);
                    int itemIndex = generator.IndexFromGeneratorPosition(childGeneratorPos);

                    if (itemIndex < firstVisibleIndex || itemIndex > lastVisibleIndex)
                    {
                        generator.Remove(childGeneratorPos, 1);
                        RemoveInternalChildRange(i, 1);
                    }
                }
            }
        }

        #region Layout
        /// <summary>
        /// Calculate the extent of the view
        /// </summary>
        private Size CalculateExtent()
        {
            int columns = DataSource.Columns.Length;
            int rows = DataSource.Rows.Length * DataSource.RowProperties.Length;

            return new Size(PropertiesGridControl.DataItemWidth * columns,
                PropertiesGridControl.RowHeight * rows);
        }

        /// <summary>
        /// Get the range of children that are visible
        /// </summary>
        /// <param name="firstVisibleItemIndex">The item index of the first visible item</param>
        /// <param name="lastVisibleItemIndex">The item index of the last visible item</param>
        private VisibleRange GetVisibleRange()
        {
            ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
            int itemCount = itemsControl.HasItems ? itemsControl.Items.Count : 0;

            if (itemCount == 0)
            {
                return VisibleRange.Empty;
            }
            else { 
                int columns = DataSource.Columns.Length;

                VisibleRange range = new VisibleRange();
                range.firstRow = Math.Max(0,(int)Math.Floor(_offset.Y / PropertiesGridControl.RowHeight) - CREATE_MARGIN_ELEMENTS);
                range.firstCol = (int)Math.Floor(_offset.X / PropertiesGridControl.DataItemWidth);
                range.rowCount = (int)Math.Ceiling((_viewport.Height) / PropertiesGridControl.RowHeight) + (CREATE_MARGIN_ELEMENTS*2) + 1; //Anstatt +1 wäre korrekterweise der ausgeblendete Teil der ersten Zeile zu beachten
                range.colCount = (int)Math.Ceiling((_viewport.Width) / PropertiesGridControl.DataItemWidth) + 1; //--||--

                int lastIndex = (range.firstRow + range.rowCount) * columns + range.firstCol + range.colCount;
                if (lastIndex >= itemCount)
                {
                    //Anzeige kann nicht komplett ausgefüllt werden
                    int maxRowCount = Math.Max(0, itemCount / columns - range.firstRow);
                    range.rowCount =  Math.Min(maxRowCount, range.rowCount);
                }
                return range;
            }
        }

        /// <summary>
        /// Get the size of the children. We assume they are all the same
        /// </summary>
        /// <returns>The size</returns>
        private Size GetChildSize()
        {
            return new Size(PropertiesGridControl.DataItemWidth, PropertiesGridControl.RowHeight);
        }
        #endregion

        #region IScrollInfo
        private TranslateTransform _trans = new TranslateTransform();
        private ScrollViewer _owner;
        private bool _canHScroll = false;
        private bool _canVScroll = false;
        private Size _extent = new Size(0, 0);
        private Size _viewport = new Size(0, 0);
        private Point _offset;

        public bool CanHorizontallyScroll
        {
            get
            {
                return _canHScroll;
            }
            set
            {
                _canHScroll = value;
            }
        }

        public bool CanVerticallyScroll
        {
            get
            {
                return _canVScroll;
            }
            set
            {
                _canVScroll = value;
            }
        }

        public double ExtentHeight
        {
            get { return _extent.Height; }
        }

        public double ExtentWidth
        {
            get { return _extent.Width; }
        }

        public double HorizontalOffset
        {
            get { return _offset.X; }
        }

        public double VerticalOffset
        {
            get { return _offset.Y; }
        }

        public double ViewportHeight
        {
            get { return _viewport.Height; }
        }

        public double ViewportWidth
        {
            get { return _viewport.Width; }
        }

        public void LineDown()
        {
            SetVerticalOffset(this.VerticalOffset + 10);
        }

        public void LineLeft()
        {
            SetHorizontalOffset(this.HorizontalOffset + 10);
        }

        public void LineRight()
        {
            SetHorizontalOffset(this.HorizontalOffset - 10);
        }

        public void LineUp()
        {
            SetVerticalOffset(this.VerticalOffset - 10);
        }

        public void MouseWheelDown()
        {
            SetVerticalOffset(this.VerticalOffset + 10);
        }

        public void MouseWheelLeft()
        {
            SetHorizontalOffset(this.HorizontalOffset + 10);
        }

        public void MouseWheelRight()
        {
            SetHorizontalOffset(this.HorizontalOffset - 10);
        }

        public void MouseWheelUp()
        {
            SetVerticalOffset(this.VerticalOffset - 10);
        }

        public void PageDown()
        {
            SetVerticalOffset(this.VerticalOffset + _viewport.Height);
        }

        public void PageLeft()
        {
            SetHorizontalOffset(this.HorizontalOffset + _viewport.Width);
        }

        public void PageRight()
        {
            SetHorizontalOffset(this.HorizontalOffset - _viewport.Width);
        }

        public void PageUp()
        {
            SetVerticalOffset(this.VerticalOffset - _viewport.Height);
        }

        public System.Windows.Rect MakeVisible(System.Windows.Media.Visual visual, System.Windows.Rect rectangle)
        {
            return new Rect();
        }

        public ScrollViewer ScrollOwner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        public void SetHorizontalOffset(double offset)
        {
            if (offset < 0 || _viewport.Width >= _extent.Width)
            {
                offset = 0;
            }
            else
            {
                if (offset + _viewport.Width >= _extent.Width)
                {
                    offset = _extent.Width - _viewport.Width;
                }
            }

            _offset.X = offset;

            if (_owner != null)
                _owner.InvalidateScrollInfo();

            _trans.X = -offset;

            // Force us to realize the correct children
            InvalidateMeasure();
        }

        public void SetVerticalOffset(double offset)
        {
            if (offset < 0 || _viewport.Height >= _extent.Height)
            {
                offset = 0;
            }
            else
            {
                if (offset + _viewport.Height >= _extent.Height)
                {
                    offset = _extent.Height - _viewport.Height;
                }
            }

            _offset.Y = offset;

            if (_owner != null)
                _owner.InvalidateScrollInfo();

            _trans.Y = -offset;

            // Force us to realize the correct children
            InvalidateMeasure();
        }

        private void UpdateScrollInfo(Size availableSize)
        {
            // See how many items there are
            ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
            int itemCount = itemsControl.HasItems ? itemsControl.Items.Count : 0;

            Size extent = CalculateExtent();
            // Update extent
            if (extent != _extent)
            {
                _extent = extent;
                if (_owner != null)
                    _owner.InvalidateScrollInfo();
            }

            // Update viewport
            if (availableSize != _viewport)
            {
                _viewport = availableSize;
                if (_owner != null)
                    _owner.InvalidateScrollInfo();
            }
        }

        #endregion
    }
}
