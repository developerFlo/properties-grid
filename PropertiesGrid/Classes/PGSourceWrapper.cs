using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGrid.Classes
{
    class PGSourceWrapper:IPGSource
    {
        IPGSource _source;
        PGItem[] _items;

        private PGSourceWrapper(IPGSource source)
        {
            this._source = source;
        }

        public static PGSourceWrapper Default
        {
            get
            {
                return new PGSourceWrapper(null)
                {
                    _items = new PGItem[0]
                };
            }
        }

        public PGItem[] Items
        {
            get { return _items; }
        }

        public static Task<PGSourceWrapper> CreateWrapperAsync(IPGSource source)
        {
            IPGSource src = source;
            return Task.Factory.StartNew(() =>
            {
                return CreateWrapper(src);
            });
        }

        private static PGSourceWrapper CreateWrapper(IPGSource source)
        {
            PGSourceWrapper wrapper = new PGSourceWrapper(source);
            int rows = source.Rows.Length;
            int cols = source.Columns.Length;
            int props = source.RowProperties.Length;
            wrapper._items = new PGItem[source.Rows.Length * source.Columns.Length * source.RowProperties.Length];
            int index = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int p = 0; p < props; p++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        wrapper._items[index++] = new PGItem(c, r, p);
                    }
                }
            }
            return wrapper;
        }

        public IPGRow[] Rows
        {
            get
            {
                if (_source == null || _source.Rows == null) return new IPGRow[0];
                else return _source.Rows;
            }
        }

        public IPGColumn[] Columns
        {
            get
            {
                if (_source == null || _source.Columns == null) return new IPGColumn[0];
                else return _source.Columns;
            }
        }

        public IPGRowPropertyDef[] RowProperties
        {
            get
            {
                if (_source == null || _source.RowProperties == null) return new IPGRowPropertyDef[0];
                else return _source.RowProperties;
            }
        }
    }
}
