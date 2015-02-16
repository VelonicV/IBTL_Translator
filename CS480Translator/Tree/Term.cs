using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tree
{
    class Term : IParseTree
    {
        private Tokens.GenericToken data;

        public Term(Tokens.GenericToken data)
        {
            this.data = data;
        }

        public Tokens.GenericToken getData()
        {
            return data;
        }
    }
}
