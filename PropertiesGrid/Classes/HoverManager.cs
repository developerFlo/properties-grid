using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PropertiesGrid.Classes
{
    class HoverManager
    {
        PropertiesGridControlViewModel _vm;

        HoverState _curState = new HoverState();

        public HoverManager(PropertiesGridControlViewModel vm)
        {
            this._vm = vm;
        }

        public void Reset()
        {
            _curState = new HoverState();
        }

        private void RefreshHover(HoverState newState)
        {
            if (!_curState.Equals(newState))
            {
                if (_curState.rowIndex != null)
                {
                    this._vm.Rows[_curState.rowIndex.Value].IsHovered = false;
                    if (_curState.propIndex != null)
                        this._vm.Rows[_curState.rowIndex.Value].Properties[_curState.propIndex.Value].IsHovered = false;
                }
                if (newState.rowIndex != null)
                {
                    this._vm.Rows[newState.rowIndex.Value].IsHovered = true;
                    if (newState.propIndex != null)
                        this._vm.Rows[newState.rowIndex.Value].Properties[newState.propIndex.Value].IsHovered = true;
                }


                if (_curState.columnIndex != null)
                    this._vm.Columns[_curState.columnIndex.Value].IsHovered = false;
                if (newState.columnIndex != null)
                    this._vm.Columns[newState.columnIndex.Value].IsHovered = true;


                if (_curState.itemIndex != null)
                    this._vm.Items[_curState.itemIndex.Value].IsHovered = false;
                if (newState.itemIndex != null)
                    this._vm.Items[newState.itemIndex.Value].IsHovered = true;
                

                _curState = newState;
            }
        }

        public void RegisterItem(UIElement child)
        {
            RegisterIsMouseOverEvent<ItemViewModel>(child,(s) =>{
                return new HoverState()
                {
                    columnIndex = s.ColIndex,
                    rowIndex = s.RowIndex,
                    propIndex = s.PropIndex,
                    itemIndex = s.ItemIndex
                };
            });
        }

        public void RegisterProperty(UIElement child)
        {
            RegisterIsMouseOverEvent<RowPropertyViewModel>(child, (s) =>
            {
                return new HoverState()
                {
                    rowIndex = s.RowIndex,
                    propIndex = s.PropIndex
                };
            });
        }

        public void RegisterRow(UIElement child)
        {
            RegisterIsMouseOverEvent<RowViewModel>(child, (s) =>
            {
                return new HoverState()
                {
                    rowIndex = s.RowIndex
                };
            });
        }

        public void RegisterColumn(UIElement child)
        {
            RegisterIsMouseOverEvent<ColumnViewModel>(child, (s) =>
            {
                return new HoverState()
                {
                    columnIndex = s.ColumnIndex
                };
            });
        }

        private void RegisterIsMouseOverEvent<T>(UIElement child, Func<T,HoverState> isMouseOverChanged) where T : class
        {
            System.ComponentModel.DependencyPropertyDescriptor.FromProperty(FrameworkElement.IsMouseOverProperty, typeof(FrameworkElement))
                .AddValueChanged(child, (s, e) =>
                {
                    HoverState newHoverState = new HoverState();
                    FrameworkElement elem = (FrameworkElement)s;
                    if (elem.IsMouseOver)
                    {
                        newHoverState = isMouseOverChanged((T)elem.DataContext);
                    }
                    RefreshHover(newHoverState);
                });
        }
    }

    struct HoverState
    {
        public int? columnIndex;
        public int? rowIndex;
        public int? propIndex;
        public int? itemIndex;
    }
}
