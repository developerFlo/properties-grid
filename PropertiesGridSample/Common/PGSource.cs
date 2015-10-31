using PropertiesGrid.Interfaces;
using PropertiesGridSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGridSample.Common
{
    class PGSource : IPGSource
    {
        const int ROW_COUNT = 100;
        static readonly int? COLUMN_COUNT = null;

        public PGSource()
        {
            Random r = new Random();

            //Create Columns
            int columnsCount = COLUMN_COUNT??Enumerable.Range(1, 12).Select(month => DateTime.DaysInMonth(DateTime.Today.Year, month)).Sum();
            Date[] columns = new Date[columnsCount];
            DateTime iter = new DateTime(DateTime.Today.Year, 1, 1);
            for(int i = 0; i < columns.Length; i++)
            {
                columns[i] = new Date(iter);
                iter = iter.AddDays(1);
            }
            Columns = columns;

            //Create some Rows
            Person[] rows = new Person[ROW_COUNT];
            string[] samplePlaces = new string[] { "Austria", "Germany", "Netherlands", "Italy", "France", "Spain", "Russia", "Japan", "Greece" };
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new Person("First", "Last " + i.ToString());
                rows[i].DayInfos = new DayInfo[columnsCount];
                for (int d = 0; d < columnsCount; d++)
                {
                    bool holiday = r.Next(365) < 20;
                    rows[i].DayInfos[d] = new DayInfo()
                    {
                        WorkHours = holiday ? 0 : Convert.ToDecimal(r.NextDouble() * 9),
                        BreakTime = holiday ? 0 : Convert.ToDecimal(r.NextDouble() * 0.5),
                        GamingHours = r.Next(1) == 1 ? Convert.ToDecimal(r.NextDouble() * 2) : 0,
                        OnHoliday = holiday,
                        Sik = r.Next(365) < 8,
                        StayAt = samplePlaces[r.Next(samplePlaces.Length)]
                    };
                }
            }
            Rows = rows;
        }

        public IPGColumn[] Columns
        {
            get; set;
        }
        
        public IPGRow[] Rows
        {
            get; set;
        }
    }
}
