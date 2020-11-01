using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseLib
{
    class ConstDefinitions
    {
        public const int _maxAmount = 100;
        public const int _minAmount = 10;
        public const int _expiry = 30;//days, removing of boxes which have not been purchased more than the above number of days
    }
}
