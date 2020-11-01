using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseLib
{
    class BoxData : IComparable<BoxData>
    {
        public int AmountOfStock { get; set; }
        public QueueTime.TimeNode NodeQueue { get; set; }//reference to the node of queue of WareHouse class which belong to this box

        public BoxData(QueueTime.TimeNode queueNode)
        {
            NodeQueue = queueNode;
        }
        public BoxData(int amountOfStock, QueueTime.TimeNode queueNode) : this(queueNode)
        {
            AmountOfStock = amountOfStock;
        }

        //getters of height and bottom of box, from nodeQueue 
        public double Height
        {
            get { return NodeQueue.TimeData.Height; }
        }
        public double Bottom
        {
            get { return NodeQueue.TimeData.SideButtom; }
        }

        //comparing two boxes by height
        //the adding of box is to BoxButtom which is sorted by height 

        public int CompareTo(BoxData other)
        {
            return NodeQueue.TimeData.Height.CompareTo(other.NodeQueue.TimeData.Height);
        }

        public override string ToString()
        {
            StringBuilder details = new StringBuilder("Details:");
            details.AppendLine("Bottom:").Append(NodeQueue.TimeData.SideButtom);
            details.AppendLine("Height:").Append(NodeQueue.TimeData.Height);
            details.AppendLine("Last purchase date:").Append(NodeQueue.TimeData.LastPurchaseDate);
            details.AppendLine("Amount of stock:").Append(AmountOfStock);
            return details.ToString();
        }
    }
}
