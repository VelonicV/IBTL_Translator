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
        private int line;
        private int character;

        public Term(Tokens.GenericToken data, int line, int character)
        {
            this.data = data;
            this.line = line;
            this.character = character;
        }

        public Tokens.GenericToken getData()
        {
            return data;
        }

        public int getLine() {
            return line;
        }

        public int getCharacter() {
            return character;
        }

        public override string ToString()
        {
            return data.word;
        }
    }
}
