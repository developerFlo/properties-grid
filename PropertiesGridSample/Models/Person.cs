using PropertiesGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesGridSample.Models
{
    class Person : IPGRow
    {
        string _firstName;
        string _lastName;

        DayInfo[] _dayInfos;

        public Person(string firstName, string lastName)
        {
            this._firstName = firstName;
            this._lastName = lastName;
            this._dayInfos = new DayInfo[0];
        }

        public DayInfo[] DayInfos
        {
            get { return _dayInfos; }
            set { _dayInfos = value; }
        }

        public IPGItem[] Items
        {
            get
            {
                return _dayInfos;
            }
        }

        public string Name
        {
            get { return this.ToString(); }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", _firstName, _lastName);
        }
    }
}
