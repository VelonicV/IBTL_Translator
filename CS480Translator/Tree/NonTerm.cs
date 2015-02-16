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
        private List<IParseTree> list;

        public NonTerm(NonTerm parent)
        {
            this.parent = parent;
            list = new List<IParseTree>();
        }

        public void add(IParseTree node)
        {
            list.Add(node);
        }

        public NonTerm getParent()
        {
            return parent;
        }

        public List<IParseTree> getList()
        {
            return list;
        }

    }
}
