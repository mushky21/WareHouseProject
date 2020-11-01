using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseLib
{
    class QueueTime : IEnumerable
    {
        public TimeNode Start { get; private set; }
        public TimeNode End { get; private set; }

        //add timeData as a new node to queue
        public void EnQueue(TimeData timeData)
        {
            TimeNode newNode = new TimeNode(timeData);
            if (End == null)
            {

                Start = End = newNode;
            }
            else
            {
                End.Next = newNode;
                newNode.Previous = End;
                End = newNode;
            }
        }

        //remove the first node from queue 
        //return false if the queue is empty, else return true
        //externalizing the removed TimeData by out
        public bool DeQueue(out TimeData removedTimeData)
        {
            if (Start == null)
            {
                removedTimeData = default(TimeData);
                return false;
            }
            else
            {
                removedTimeData = Start.TimeData;
                if (Start.Next == null)
                {
                    Start = End = null;
                }
                else
                {
                    Start = Start.Next;
                    Start.Previous = null;
                }
                return true;
            }
        }

        public bool IsEmpty()
        {
            return Start == null;
        }

        public IEnumerator GetEnumerator()
        {
            TimeNode tmp = Start;
            while (tmp != null)
            {
                yield return tmp.TimeData;
                tmp = tmp.Next;
            }
        }

        //write here because we need public set of _end, in order to remove 
        //if we will write it out of class and it is not good
        //and also more right for 

        //renmove a node from queue by o(c)
        public void Remove(TimeNode nodeQueue) //count drop to zero
        {
            if (nodeQueue.Previous != null && nodeQueue.Next != null)// the node is not in start or end of queue
            {
                nodeQueue.Next.Previous = nodeQueue.Previous;
                nodeQueue.Previous.Next = nodeQueue.Next;
            }
            else if (Start == nodeQueue)  //the node at the start or the node also in the end, because there are no additional nodes 
            {
                //or to use dequeue?
                if (Start == End) Start = End = null;
                else
                {
                    Start = Start.Next;
                    Start.Previous = null;
                }
            }
            else //the node at the end of queue and not in start
            {
                End = End.Previous;
                End.Next = null;
            }
        }

        //move a node to the end of the queue by o(c)
        //to return bool if executed a movement to end?
        //public void MoveToEnd(TimeNode nodeQueue)
        //{
        //    //check if executed a removing
        //    //there is no removing, when the node is in end of queue or there are no additional
        //    //nodes except this node
        //    bool isRemoved = false;

        //    if (nodeQueue.Previous != null && nodeQueue.Next != null)// the node is not in start or end of queue
        //    {
        //        nodeQueue.Next.Previous = nodeQueue.Previous;
        //        nodeQueue.Previous.Next = nodeQueue.Next;
        //        isRemoved = true;
        //    }
        //    else if (End != nodeQueue)//the node is in start of queue
        //    {
        //        Start = Start.Next;
        //        Start.Previous = null;
        //        isRemoved = true;
        //    }
        //    if (isRemoved) //uses the original reference  of node and add  it to the end
        //    {
        //        nodeQueue.Previous = End;
        //        nodeQueue.Next = null;
        //        End.Next = nodeQueue;             
        //        End = nodeQueue;
        //    }
        //}
        public void MoveToEnd(TimeNode nodeQueue)
        {
            if(nodeQueue!=End)
            {
                Remove(nodeQueue);
                nodeQueue.Previous = End;
                nodeQueue.Next = null;
                End.Next = nodeQueue;
                End = nodeQueue;
            }
        }

        internal class TimeNode
        {
            public TimeData TimeData { get; set; }//the height,buttom and lest purchased date
            public TimeNode Previous { get; set; }
            public TimeNode Next { get; set; }
            public TimeNode(TimeData timeData)
            {
                TimeData = timeData;
            }
        }

        //??
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}
    }
}
