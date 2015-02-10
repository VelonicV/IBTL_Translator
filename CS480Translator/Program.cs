using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    class Program
    {
        static void Main(string[] args)
        {

            SymbolTable st = new SymbolTable();
            Lexalizer lex = new Lexalizer(args[0]);
            Tokens.GenericToken token = lex.getNextToken();

            while (token != null)
            {
                if (token is Tokens.IT)
                {
                    st.add((Tokens.IT) token);
                }

                Console.WriteLine(token.ToString());
                token = lex.getNextToken();
            }

            Console.WriteLine("\nSymbol Table entries: {0}", st.count());
            //Console.ReadLine();

        }
    }
}
