using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    class Node
    {
        //Stores the first child in the child singly linked-list, its next sibling, and the token.
        public Node firstChild;
        public Node nextSibling;
        public Tokens.GenericToken data;

        //Constructor expects the parent node (can be null for the root), and the token to store.
        public Node(Node parent, Tokens.GenericToken data)
        {
            //Initially set the class members.
            firstChild = null;
            nextSibling = null;
            this.data = data;

            //If not the root, set self as the last sibling.
            if (parent != null)
            {
                updateParent(parent);
            }

        }

        //Updates the parent's child linked list, adding self to the end.
        private void updateParent(Node parent)
        {
            if (parent.firstChild == null)
            {
                parent.firstChild = this;
            }
            else
            {
                Node sibling = parent.firstChild;
                while (sibling.nextSibling != null)
                {
                    sibling = sibling.nextSibling;
                }

                sibling.nextSibling = this;
            }
        }

        //Traverse the tree starting at a given node in post order.
        public static void postOrderTraversal(Node node)
        {
            Node linkedList = node.firstChild;
            while (linkedList != null)
            {
                postOrderTraversal(linkedList);
                linkedList = linkedList.nextSibling;
            }
            if (node.data != null)
            {
                Console.WriteLine(node.data.ToString());
            }
        }

    }
}
