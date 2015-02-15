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

            //SymbolTable st = new SymbolTable();
            //Lexalizer lex = new Lexalizer(args[0]);
            //Tokens.GenericToken token = lex.getNextToken();

            //while (token != null)
            //{
            //    if (token is Tokens.IT)
            //    {
            //        st.add((Tokens.IT) token);
            //    }

            //    Console.WriteLine(token.ToString());
            //    token = lex.getNextToken();
            //}

            //Console.WriteLine("\nSymbol Table entries: {0}", st.count());
            ////Console.ReadLine();

            //Node root = new Node(null, null);
            //Node rootChild1 = new Node(root, new Tokens.IT("test"));
            //Node rootChild2 = new Node(root, new Tokens.IT("test2"));
            //Node rootChild3 = new Node(root, new Tokens.IT("test3"));
            //Node rootChild2Child1 = new Node(rootChild2, new Tokens.IT("test4"));
            //Node rootChild2Child2 = new Node(rootChild2, new Tokens.IT("test5"));

            //Node.postOrderTraversal(root);
            //Console.Read();

            Parser parser = new Parser(args[0]);
            Console.ReadLine();

        }
    }
}
