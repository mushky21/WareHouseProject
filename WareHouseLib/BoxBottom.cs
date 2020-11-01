using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseLib
{
    class BoxBottom : IComparable<BoxBottom>
    {
        public BST<BoxData> BoxHeight { get; set; }
        public double Bottom { get; set; }//The width of bottom of all boxes in the current BST

        public BoxBottom(double x, bool isAddBoxHeight = false)
        {
            if (isAddBoxHeight) BoxHeight = new BST<BoxData>();
            Bottom = x;
        }

        public void AddBox(BoxData box)
        {
            BoxHeight.Add(box);
        }
        public bool FindBox(out BoxData foundBox, BoxData boxForSearch)
        {
            return BoxHeight.Find(out foundBox, boxForSearch);
        }
        public int CompareTo(BoxBottom other)
        {
            return this.Bottom.CompareTo(other.Bottom);
        }
        public override bool Equals(object obj)
        {
            return this.Bottom.Equals((obj as BoxBottom).Bottom);
        }
    }
}
