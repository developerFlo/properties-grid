using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGridSample.Models
{
    class DayInfo:IPGItem
    {
        decimal _workHours;
        decimal _breakTime;
        bool _onHoliday;
        bool _sik;
        string _stayAt;
        decimal _gamingHours;

        public decimal WorkHours
        {
            get
            {
                return _workHours;
            }

            set
            {
                _workHours = value;
            }
        }

        public decimal BreakTime
        {
            get
            {
                return _breakTime;
            }

            set
            {
                _breakTime = value;
            }
        }

        public bool OnHoliday
        {
            get
            {
                return _onHoliday;
            }

            set
            {
                _onHoliday = value;
            }
        }

        public bool Sik
        {
            get
            {
                return _sik;
            }

            set
            {
                _sik = value;
            }
        }

        public string StayAt
        {
            get
            {
                return _stayAt;
            }

            set
            {
                _stayAt = value;
            }
        }

        public decimal GamingHours
        {
            get
            {
                return _gamingHours;
            }

            set
            {
                _gamingHours = value;
            }
        }

        public IPGItem DeepCopy()
        {
            DayInfo di = new DayInfo();
            di._breakTime = this._breakTime;
            di._gamingHours = this._gamingHours;
            di._onHoliday = this._onHoliday;
            di._sik = this._sik;
            di._stayAt = this._stayAt;
            di._workHours = this._workHours;
            return di;
        }

        public void ResetValues(IPGItem item)
        {
            DayInfo di = item as DayInfo;
            if(item != null)
            {
                this.BreakTime = di._breakTime;
                this.GamingHours = di._gamingHours;
                this.OnHoliday = di._onHoliday;
                this.Sik = di._sik;
                this.StayAt = di._stayAt;
                this.WorkHours = di._workHours;
            }
        }
    }
}
