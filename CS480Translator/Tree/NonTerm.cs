using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tree
{
    class NonTerm : IParseTree
    {
        private List<IParseTree> parent;
        private List<IParseTree> list;

        public NonTerm(List<IParseTree> parent)
        {
            this.parent = parent;
            list = new List<IParseTree>();
        }

        public void add(IParseTree node)
        {
            list.Add(node);
        }

        public List<IParseTree> getParent()
        {
            return parent;
        }

    }
}
