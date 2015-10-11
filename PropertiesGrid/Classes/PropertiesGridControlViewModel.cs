using PropertiesGrid.Control;
using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PropertiesGrid.Classes
{
    class PropertiesGridControlViewModel:INotifyPropertyChanged
    {
        IPGSource _source;
        ItemViewModel[] _items;
        RowViewModel[] _rows;
        ColumnViewModel[] _columns;
        ObservableCollection<RowProperty> _props;

        public PropertiesGridControlViewModel()
        {
            _source = new PGSourceStub();
            _items = new ItemViewModel[0];
            _rows = new RowViewModel[0];
            Columns = new ColumnViewModel[0];
            _props = new ObservableCollection<RowProperty>();
        }
        
        public ItemViewModel[] Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged("Items");
                }
            }
        }
        public RowViewModel[] Rows
        {
            get { return _rows; }
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    RaisePropertyChanged("Rows");
                }
            }
        }

        public IPGSource Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    RaisePropertyChanged("Source");
                }
            }
        }

        public ObservableCollection<RowProperty> Props
        {
            get
            {
                return _props;
            }

            set
            {
                if (_props != value)
                {
                    _props = value;
                    RaisePropertyChanged("Props");
                }
            }
        }

        internal ColumnViewModel[] Columns
        {
            get
            {
                return _columns;
            }

            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    RaisePropertyChanged("Columns");
                }
            }
        }

        public void RebaseOnSource(DataTemplate rowTemplate, DataTemplate propertyTemplate, DataTemplate columnTemplate)
        {
            int rowCount = this.Source.Rows.Length;
            int propCount = this.Props.Count;
            int colCount = this.Source.Columns.Length;
            RowViewModel[] rows = new RowViewModel[rowCount];
            ColumnViewModel[] columns = new ColumnViewModel[colCount];
            ItemViewModel[] items = new ItemViewModel[rowCount * propCount * colCount];
            int itemIndex = 0;
            for (int r = 0; r < rowCount; r++)
            {
                RowViewModel row = new RowViewModel();
                row.Row = this.Source.Rows[r];
                row.Properties = new RowPropertyViewModel[propCount];
                row.HeaderTemplate = rowTemplate;
                for (int p = 0; p < propCount; p++)
                {
                    RowPropertyViewModel prop = new RowPropertyViewModel(this.Props[p], propertyTemplate);
                    row.Properties[p] = prop;
                    for (int c = 0; c < colCount; c++)
                    {
                        items[itemIndex++] = new ItemViewModel(c, r, p);
                    }
                }
                rows[r] = row;
            }
            for (int c = 0; c < colCount; c++)
            {
                columns[c] = new ColumnViewModel(this.Source.Columns[c], columnTemplate);
            }

            this.Rows = rows;
            this.Columns = columns;
            this.Items = items;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
