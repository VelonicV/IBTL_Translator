using System.Collections;

namespace CS480Translator
{
    
    class SymbolTable
    {
        private Hashtable hash;
        private SymbolTable parent;

        public SymbolTable(SymbolTable parent)
        {
            hash = new Hashtable();
            this.parent = parent;
        }

        public void add(Tokens.IT token)
        {
            hash[token.word] = token;
        }

        public Tokens.IT lookup(string idName)
        {
            return (Tokens.IT) hash[idName];
        }

        public int count()
        {
            return hash.Count;
        }

    }
}
