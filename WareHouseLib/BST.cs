using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseLib
{
    class BST<T> where T : IComparable<T>
    {
        private Node Root;

        public bool IsEmpty()
        {
            if (Root==null) return true;
            return false;
        }

        public bool Find(out T findItem, T itemForSearch)
        {
            Node tmp = Root;
            findItem = default(T);
            while (tmp != null)
            {
                if (tmp.data.CompareTo(itemForSearch) == 0)
                {
                    findItem = tmp.data;
                    return true;
                }
                else if (itemForSearch.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }
            return false;
        }

        //find equal or the closest sequental item
        public bool FindClosest(T item, out T itemFounded)
        {
            bool isBiggest = false;//if there are biggest from item
            Node tmp = Root;
            itemFounded = default(T);
            while (tmp != null)
            {
                if (tmp.data.CompareTo(item) == 0)
                {
                    itemFounded = tmp.data;
                    return true;
                }
                else if (item.CompareTo(tmp.data) < 0)
                {
                    itemFounded = tmp.data;
                    isBiggest = true;//not must in all itery
                    itemFounded = tmp.data;
                    tmp = tmp.left;
                }
                else tmp = tmp.right;
            }
            return isBiggest;
        }

        public void Add(T item)
        {
            Node newNode = new Node(item);
            if (Root == null)
            {
                Root = newNode;
                return;
            }
            Node tmp = Root;
            Node parent = null;

            while (tmp != null)
            {
                parent = tmp;
                if (newNode.data.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }
            if (newNode.data.CompareTo(parent.data) < 0) // add to left
                parent.left = newNode;
            else
                parent.right = newNode;
            //list - add to list
        }

        public bool Remove(T value, out T removedItem)
        {
            removedItem = default(T);
            if (Root == null) return false;
            Node tmp = Root;
            Node parent = Root;
            while (tmp != null)//searching of the value
            {
                if (value.CompareTo(tmp.data) < 0)
                {
                    parent = tmp;
                    tmp = tmp.left;
                }
                else if (value.CompareTo(tmp.data) > 0)
                {
                    parent = tmp;
                    tmp = tmp.right;
                }
                else
                {
                    break;
                }
            }
            if (tmp == null) return false;//if the value is not found
            removedItem = tmp.data;
            //check if tmp has no childs
            if (tmp.left == null && tmp.right == null)
            {
                Delete(parent, tmp, null);
            }
            //check if tmp has two childs
            else if (tmp.right != null && tmp.left != null)
            {
                //Search for the next smallest value after the value you want to remove
                Node parentForTwoChilds = tmp;//parent of the right child of tmp (the node for removing)
                Node tmpForTwoChilds = tmp.right;
                while (tmpForTwoChilds.left != null)
                {
                    parentForTwoChilds = tmpForTwoChilds;
                    tmpForTwoChilds = tmpForTwoChilds.left;
                }
                tmp.data = tmpForTwoChilds.data;//the smallest value in the right child of tmp for overriding the value for removing
                if (tmpForTwoChilds.right != null)
                {
                    Delete(parentForTwoChilds, tmpForTwoChilds, tmpForTwoChilds.right);
                }
                else
                {
                    Delete(parentForTwoChilds, tmpForTwoChilds, null);
                }
            }
            else
            {
                if (tmp.left != null)
                {
                    Delete(parent, tmp, tmp.left);

                }
                else
                {
                    Delete(parent, tmp, tmp.right);
                }
            }
            return true;
        }

        private void Delete(Node parent, Node childOrNot, Node newNode)
        {
            if (parent.left == childOrNot)
            {
                parent.left = newNode;
            }
            else if (parent.right == childOrNot) parent.right = newNode;
            else Root = newNode;
        }

        public void PrintInOrder()
        {
            PrintInOrder(Root);
        }

        private void PrintInOrder(Node n)
        {
            if (n == null) return;

            PrintInOrder(n.left);
            Console.WriteLine(n.data);
            PrintInOrder(n.right);
        }

        public int CountNodes()
        {
            return CountNodes(Root);
        }

        private int CountNodes(Node n)
        {
            if (Root == null) return 0;
            else return 1 + CountNodes(n.left) + CountNodes(n.right);
        }

        private bool Equals(Node n, Node v)
        {
            if (n == null && v == null) return true;
            else if (n == null || v == null) return false;
            else if (!(n.data).Equals(v.data)) return false;
            else return Equals(n.left, v.left) && Equals(n.right, v.right);
        }

        public override bool Equals(object bst)
        {
            return Equals(this.Root, (bst as BST<T>).Root);
        }

        //public IEnumerable<T> GetEnumerator()
        //{
        //    return GetEnumerator(Root);
        //}
        //private IEnumerable<T> GetEnumerator(Node n)
        //{
        //    foreach (var item in GetEnumerator(n.left))
        //    {
        //        yield return (T)item;
        //    }
        //    yield return n.data;
        //    foreach (var item in GetEnumerator(n.right))
        //    {
        //        yield return (T)item;
        //    }
        //}



        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    yield return GetEnumerator();
        //}

        //public IEnumerator<T> GetEnumerator()
        //{
        //    return GetEnumerator(Root);
        //}
        //private IEnumerator<T> GetEnumerator(Node n)
        //{
        //        GetEnumerator(n.left);
        //        yield return n.data;
        //        GetEnumerator(n.right);        
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        class Node
        {
            public T data { get; set; }
            public Node left { get; set; }
            public Node right { get; set; }

            public Node(T data)
            {
                this.data = data;
            }
            public override bool Equals(object obj)
            {
                return this.data.Equals((obj as Node).data);
            }
        }
    }
}
