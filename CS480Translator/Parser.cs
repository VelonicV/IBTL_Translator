using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    class Parser
    {
        //Class members.
        private Node root;
        private Lexalizer lex;
        private SymbolTable st;
        Tokens.GenericToken prev;
        Tokens.GenericToken next;
        int tabs;

        //Initialize class variables
        public Parser(string filePath)
        {
            root = new Node(null, null);
            st = new SymbolTable();
            tabs = 0;

            lex = new Lexalizer(filePath);
            next = lex.getNextToken();

            S();
            
            if(!(next is Tokens.EOFT))
            {
                throw new Exception("Grammatical Error: invalid token '" + next.word + "' following token '" + prev.word + "' found on line " + lex.getLine() + ".");
            }

        }

        // S -> ( S" | constants S' | name S'
        private void S()
        {
            if (TokenEquiv.isLP(next))
            {
                pan();
                tabs++;
                SPP();
                
            }
            else if (TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                pan();
                SP();
            }
            else
            {
                err();
            }
            
        }

        // S' -> S S' | EPSILON
        private void SP()
        {
            
            if (TokenEquiv.isEOF(next) || TokenEquiv.isRP(next))
            {
                //EPSILON
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                S();
                SP();
            }

            else
            {
                err();
            }
            
        }

        // S" -> ) S' | S ) S' | expr' S'
        private void SPP()
        {
            
            if (TokenEquiv.isRP(next))
            {
                tabs--;
                pan();
                SP();
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                S();
                if (TokenEquiv.isRP(next))
                {
                    tabs--;
                    pan();
                    SP();
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isAssign(next) || TokenEquiv.isBinary(next) || TokenEquiv.isUnary(next) 
                   || TokenEquiv.isMinus(next) || TokenEquiv.isStdout(next) || TokenEquiv.isIf(next) 
                   || TokenEquiv.isWhile(next) || TokenEquiv.isLet(next))
            {
                exprP();
                SP();
            }
            else
            {
                err();
            }
            
        }

        // expr -> ( expr' | constants | name
        private void expr()
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan();
                tabs++;
                exprP();
                
            }
            else if (TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                pan();
            }
            else
            {
                err();
            }
            
        }

        // expr' -> oper' | stmts'
        private void exprP()
        {
            
            if (TokenEquiv.isAssign(next) || TokenEquiv.isBinary(next) || TokenEquiv.isUnary(next) || TokenEquiv.isMinus(next))
            {
                operP();
            }
            else if (TokenEquiv.isStdout(next) || TokenEquiv.isIf(next) || TokenEquiv.isWhile(next) || TokenEquiv.isLet(next))
            {
                stmtsP();
            }
            else
            {
                err();
            }
            
        }

        // oper -> ( oper' | constants | name
        private void oper()
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan();
                tabs++;
                operP();
                
            }
            else if (TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                pan();
            }
            else
            {
                err();
            }
            
        }

        // oper' -> := name oper ) | binops oper oper ) | unops oper ) | - oper oper"
        private void operP()
        {
            
            if (TokenEquiv.isAssign(next))
            {
                pan();
                if (TokenEquiv.isName(next))
                {
                    pan();
                    oper();
                    if (TokenEquiv.isRP(next))
                    {
                        tabs--;
                        pan();
                    }
                    else
                    {
                        err();
                    }
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isBinary(next))
            {
                pan();
                oper();
                oper();
                if (TokenEquiv.isRP(next))
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isUnary(next))
            {
                pan();
                oper();
                if (TokenEquiv.isRP(next))
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isMinus(next))
            {
                pan();
                oper();
                operPP();
            }
            else
            {
                err();
            }
            
        }

        // oper" -> oper ) | )
        private void operPP()
        {
            
            if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                oper();
                if (TokenEquiv.isRP(next))
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isRP(next))
            {
                tabs--;
                pan();
            }
            else
            {
                err();
            }
            
        }

        // stmts -> ( stmts'
        private void stmts()
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan();
                tabs++;
                stmtsP();
                
            }
            else
            {
                err();
            }
            
        }

        // stmts' -> printstmts | whilestmts | ifstmts | letstmts
        private void stmtsP()
        {
            
            if (TokenEquiv.isStdout(next))
            {
                printstmts();
            }
            else if (TokenEquiv.isWhile(next))
            {
                whilestmts();
            }
            else if (TokenEquiv.isIf(next))
            {
                ifstmts();
            }
            else if (TokenEquiv.isLet(next))
            {
                letstmts();
            }
            else
            {
                err();
            }
            
        }

        // printstmts -> stdout oper )
        private void printstmts()
        {
            
            if (TokenEquiv.isStdout(next))
            {
                pan();
                oper();
                if (TokenEquiv.isRP(next))
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else
            {
                err();
            }
            
        }

        // ifstmts -> if expr expr ifstmts'
        private void ifstmts()
        {
            
            if (TokenEquiv.isIf(next))
            {
                pan();
                expr();
                expr();
                ifstmtsP();
            }
            else 
            {
                err();
            }
            
        }

        // ifstmts' -> ) | expr )
        private void ifstmtsP()
        {
            
            if (TokenEquiv.isRP(next))
            {
                tabs--;
                pan();
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                expr();
                if (TokenEquiv.isRP(next))
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else
            {
                err();
            }
            
        }

        // whilestmts -> while expr exprlist )
        private void whilestmts()
        {
            
            if (TokenEquiv.isWhile(next))
            {
                pan();
                expr();
                exprlist();
                if (TokenEquiv.isRP(next))
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else
            {
                err();
            }
            
        }

        // exprlist -> expr exprlist'
        private void exprlist()
        {
            
            if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                expr();
                exprlistP();
            }
            else
            {
                err();
            }
            
        }

        // exprlist' -> exprlist | EPSILON
        private void exprlistP()
        {
            
            if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                exprlist();
            }
            else if (TokenEquiv.isRP(next))
            {
                //EPSILON
            }
            else
            {
                err();
            }
            
        }

        // letstmts -> let ( varlist ) )
        private void letstmts()
        {
            
            if (TokenEquiv.isLet(next))
            {
                pan();
                if (TokenEquiv.isLP(next))
                {
                    varlist();
                    if (TokenEquiv.isRP(next))
                    {
                        tabs--;
                        pan();
                    }
                    else
                    {
                        err();
                    }
                }
                else
                {
                    err();
                }
            }
            else
            {
                err();
            }
            
        }

        // varlist -> ( name type ) varlist'
        private void varlist()
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan();
                tabs++;
                if (TokenEquiv.isName(next))
                {
                    pan();
                    if (TokenEquiv.isType(next))
                    {
                        pan();
                        if (TokenEquiv.isRP(next))
                        {
                            tabs--;
                            pan();
                            varlistP();
                        }
                        else
                        {
                            err();
                        }
                    }
                    else
                    {
                        err();
                    }
                }
                else
                {
                    err();
                }
            }
            else
            {
                err();
            }
            
        }

        // varlist' -> varlist | EPSILON
        private void varlistP()
        {
            if (TokenEquiv.isLP(next))
            {
                varlist();
            }
            else if (TokenEquiv.isRP(next))
            {
                //EPSILON
            }
            else
            {
                err();
            }
        }

        //Generate exception on error.
        private void err()
        {
            if (TokenEquiv.isEOF(next))
            {
                throw new Exception("Grammatical Error: unexpected end of file reached on line " + lex.getLine() + ".");
            }
            else if(prev == null)
            {
                throw new Exception("Grammatical Error: invalid starting token '" + next.word + "' found on line " + lex.getLine() + ".");
            }
            else
            {
                throw new Exception("Grammatical Error: invalid token '" + next.word + "' following token '" + prev.word + "' found on line " + lex.getLine() + ".");
            }
        }

        //Print the current token, add it to the symbol table if it's an ID, and get the next token.
        private void pan()
        {
            for (int i = 0; i < tabs; i++)
            {
                Console.Write("  ");
            }
            Console.WriteLine(next.word);

            if (next is Tokens.IT)
            {
                st.add((Tokens.IT)next);
            }

            prev = next;
            next = lex.getNextToken();
        }

        //Return the built tree.
        public Node returnTree()
        {
            return root;
        }

        //Return the symbol table;
        public SymbolTable returnST()
        {
            return st;
        }

    }
}
