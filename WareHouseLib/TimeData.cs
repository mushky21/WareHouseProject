using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseLib
{
    class TimeData
    {
        public double SideButtom { get; set; }
        public double Height { get; set; }
        public DateTime LastPurchaseDate { get; set; }

        public TimeData(double height)
        {
            Height = height;
        }
        public TimeData(double sideButtom, double height)
        {
            SideButtom = sideButtom;
            Height = height;
            LastPurchaseDate = DateTime.Now;

        }
    }
}
