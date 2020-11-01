using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseLib
{
    public delegate void OnMessage(string s);

    public class WareHouse
    {
        private BST<BoxBottom> _outerTree; //the outer tree which contains nodes of BoxBottom
        private QueueTime _mostUpdatedBoxHandling;
        OnMessage onMdg;

        public WareHouse(OnMessage onMdg)
        {
            _outerTree = new BST<BoxBottom>();
            _mostUpdatedBoxHandling = new QueueTime();
            this.onMdg = onMdg;
        }

        //update stock of any box or if this box is not exist, add box with the params of method to allBoxes BST
        public void UpdateStock(double bottom, double height, int amount)
        {
            BoxBottom boxBottom;
            bool isFound = _outerTree.Find(out boxBottom, new BoxBottom(bottom));
            BoxData currentBox;//the newBox for adding or the suitable box which found

            //if there is no the requested boxBottom, add to allBoxes BST
            if (!isFound)
            {
                BoxBottom newBoxBottom = new BoxBottom(bottom, true);
                currentBox = AddBox(bottom, height, amount, newBoxBottom);
                _outerTree.Add(newBoxBottom);
                onMdg(string.Format("box {0} {1} added to stock", bottom, height));//to update that box with this buttom is not exist?
            }
            else
            {
                QueueTime.TimeNode nodeQueue = new QueueTime.TimeNode(new TimeData(height));
                isFound = boxBottom.FindBox(out currentBox, new BoxData(nodeQueue));

                // if there is no the specific height of box in suitable boxButtom, add new box 
                //with substituting the values of paramas in properties of the new box
                if (!isFound)
                {
                    currentBox = AddBox(bottom, height, amount, boxBottom);
                    onMdg(string.Format("box {0} {1} added to stock", bottom, height));
                }
                else// if there is the specific size, only update amount 
                {
                    currentBox.AmountOfStock += amount;
                    onMdg(string.Format("The amount for box {0} {1} was updated", bottom, height));
                }

            }
            //check if the amount exceeded the maximum amount which defined in ConstDefinitions class
            if (currentBox.AmountOfStock > ConstDefinitions._maxAmount)
            {
                currentBox.AmountOfStock = ConstDefinitions._maxAmount;
                onMdg(string.Format("The amount of box has been reduced to the maximum amount which is {0}", ConstDefinitions._maxAmount));
            }

        }

        //update user by message of detail's requested box 
        //if the requested box is not found - update user by suitable message
        public void ShowDetails(double bottom, double height)
        {
            BoxData boxFound;
            if (FindBox(out boxFound, bottom, height))
            {
                onMdg(boxFound.ToString());
            }
            onMdg("The requested box is not found");
        }

        // search for box that suitable to the size of any gift (acoording params bottom and height)
        public bool FindSuitable(double bottom, double height)
        {
            BoxBottom boxBottom;
            bool isFound = _outerTree.FindClosest(new BoxBottom(bottom), out boxBottom);//search by X

            if (isFound)// if there is no the side of bottom of box (value of x ,x or the smallest from all which biggest from x)
            {
                BoxData currentBox;
                QueueTime.TimeNode nodeQueue = new QueueTime.TimeNode(new TimeData(height));
                isFound = boxBottom.BoxHeight.FindClosest(new BoxData(nodeQueue), out currentBox);//serach by Y

                if (isFound)
                {
                    UpdatesForPurchase(currentBox, boxBottom);
                    return true;
                }

            }
            onMdg(string.Format("The requested box is not found"));
            return false;
        }

        //remove all boxes which are expiary
        public void RemoveExpiredBoxes()
        {
            QueueTime.TimeNode currentNode = _mostUpdatedBoxHandling.Start;
            TimeData removedTimeData;
            BoxData boxFound;
            int turnOverPeriod;

            while (currentNode != null)
            {
                turnOverPeriod = (DateTime.Now - currentNode.TimeData.LastPurchaseDate).Days;//update the substraction of currentNode
                if (turnOverPeriod < ConstDefinitions._expiry) break;

                FindBox(out boxFound, currentNode.TimeData.Height, currentNode.TimeData.SideButtom, true);//true for remove from BST
                _mostUpdatedBoxHandling.DeQueue(out removedTimeData);//remove from queue
                currentNode = _mostUpdatedBoxHandling.Start;//update currentNode to the next
            }
        }

        //private methods 

        //remove box from BST (_allBoxes)
        private void RemoveFromTree(BoxData box, BoxBottom boxBottom)
        {
            //removing from AllBoxes BST
            boxBottom.BoxHeight.Remove(box, out box);

            //removing of box bottom if the inner tree (BoxHeight) is empty
            if (boxBottom.BoxHeight.IsEmpty())//remove of the boxBottom if 
            {
                _outerTree.Remove(boxBottom, out boxBottom);//to update user??
            }
        }

        //find specific box in AllBoxes BST
        private bool FindBox(out BoxData boxFound, double bottom, double height, bool isForRemoved = false)
        {
            BoxBottom boxBottom;
            if (_outerTree.Find(out boxBottom, new BoxBottom(bottom)))
            {
                QueueTime.TimeNode nodeQueue = new QueueTime.TimeNode(new TimeData(height));
                if (boxBottom.FindBox(out boxFound, new BoxData(nodeQueue)))
                {
                    if (isForRemoved) RemoveFromTree(boxFound, boxBottom);
                    return true;
                }
            }
            boxFound = default(BoxData);
            return false;
        }

        //add box to specific BoxBottom
        private BoxData AddBox(double bottom, double height, int amount, BoxBottom boxBottom)
        {
            TimeData timeData = new TimeData(bottom, height);
            _mostUpdatedBoxHandling.EnQueue(timeData);
            BoxData boxData = new BoxData(amount, _mostUpdatedBoxHandling.End);
            boxBottom.AddBox(boxData);
            return boxData;
        }

        //update time of nodeQueue which is kept in BoxData and move node of box to ensd of queue
        private void UpdateTime(BoxData box)
        {
            QueueTime.TimeNode nodeQueue = box.NodeQueue;
            nodeQueue.TimeData.LastPurchaseDate = DateTime.Now;
            _mostUpdatedBoxHandling.MoveToEnd(nodeQueue);
        }

        //all updates for purchase of box ,including messages for user
        private void UpdatesForPurchase(BoxData box, BoxBottom boxBottom)
        {
            //update stock of box
            box.AmountOfStock--;
            double height = box.Height;
            double bottom = box.Bottom;

            //update time of box
            UpdateTime(box);

            //message for update user that the process of purchase is completed successfuly
            onMdg(string.Format("The suitable box {0} {1} was found", bottom, height));

            //check if stock for box is empty after purchase
            if (box.AmountOfStock == 0)
            {
                //remove of box with amount of 0
                _mostUpdatedBoxHandling.Remove(box.NodeQueue);//remove from queue
                RemoveFromTree(box, boxBottom);

                onMdg(string.Format("There are no boxes from box {0} {1}", bottom, height));//to update removing ?? 
            }
            else if (box.AmountOfStock <= ConstDefinitions._minAmount)//check if the amount reached the min amount - defined in ConstDefinitions class
            {
                onMdg(string.Format("Amount of box {0} {1} dropped the minimum amount (left only {2} boxes)", bottom, height, box.AmountOfStock));
            }
        }

        ////find box by height and bottom and remove from WareHouse --- for RemoveBoxesExpiary method
        //private void FindAndRemove(double height, double bottom)
        //{
        //    BoxBottom boxBottom;
        //    if (_allBoxes.Find(out boxBottom, new BoxBottom(bottom)))
        //    {
        //        BoxData boxFound;
        //        QueueTime.TimeNode nodeQueue = new QueueTime.TimeNode(new TimeData(height));
        //        if (boxBottom.FindBox(out boxFound, new BoxData(nodeQueue)))
        //        {
        //            RemoveBox(boxFound, boxBottom);//remove from queue and BST
        //        }
        //    }
        //}


    }
}
