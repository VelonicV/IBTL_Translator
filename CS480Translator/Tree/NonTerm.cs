using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tree
{
    class NonTerm : IParseTree
    {
        private NonTerm parent;
        private Queue<IParseTree> list;

        public NonTerm(NonTerm parent)
        {
            this.parent = parent;
            list = new Queue<IParseTree>();
        }

        public void add(IParseTree node)
        {
            list.Enqueue(node);
        }

        public NonTerm getParent()
        {
            return parent;
        }

        public Queue<IParseTree> getList()
        {
            return list;
        }

    }
}
