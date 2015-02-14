using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    
    class SymbolTable
    {
        Hashtable hash;

        public SymbolTable()
        {
            hash = new Hashtable();
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
