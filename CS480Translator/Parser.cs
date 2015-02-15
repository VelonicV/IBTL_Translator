using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    class Parser
    {

        private Node root;
        private Lexalizer lex;
        Tokens.GenericToken next;
        int tabs;

        public Parser(string filePath)
        {
            //root = new Node(null, null);
            lex = new Lexalizer(filePath);
            next = lex.getNextToken();
            tabs = 0;
            S();
        }

        private void S()
        {
            
            if (next.isLP())
            {
                pan();
                tabs++;
                SPP();
                
            }
            else if (next.isConstant() || next.isName())
            {
                pan();
                S();
            }
            else
            {
                err();
            }
            
        }

        private void SP()
        {
            
            if (next == null || next.isRP())
            {
                //EPSILON
            }
            else if (next.isLP() || next.isConstant() || next.isName())
            {
                S();
                SP();
            }

            else
            {
                err();
            }
            
        }

        private void SPP()
        {
            
            if (next.isRP())
            {
                tabs--;
                pan();
                SP();
            }
            else if (next.isLP() || next.isConstant() || next.isName())
            {
                S();
                if (next.isRP())
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
            else if (next.isAssign() || next.isBinary() || next.isUnary() || next.isMinus() || next.isStdout() || next.isIf() || next.isWhile() || next.isLet())
            {
                exprP();
                SP();
            }
            else
            {
                err();
            }
            
        }

        private void expr()
        {
            
            if (next.isLP())
            {
                pan();
                tabs++;
                exprP();
                
            }
            else if (next.isConstant() || next.isName())
            {
                pan();
            }
            else
            {
                err();
            }
            
        }

        private void exprP()
        {
            
            if (next.isAssign() || next.isBinary() || next.isUnary() || next.isMinus())
            {
                operP();
            }
            else if (next.isStdout() || next.isIf() || next.isWhile() || next.isLet())
            {
                stmtsP();
            }
            else
            {
                err();
            }
            
        }

        private void oper()
        {
            
            if (next.isLP())
            {
                tabs++;
                pan();
                operP();
                
            }
            else if (next.isConstant() || next.isName())
            {
                pan();
            }
            else
            {
                err();
            }
            
        }

        private void operP()
        {
            
            if (next.isAssign())
            {
                pan();
                if (next.isName())
                {
                    pan();
                    oper();
                    if (next.isRP())
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
            else if (next.isBinary())
            {
                pan();
                oper();
                oper();
                if (next.isRP())
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else if (next.isUnary())
            {
                pan();
                oper();
                if (next.isRP())
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else if (next.isMinus())
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

        private void operPP()
        {
            
            if (next.isLP() || next.isConstant() || next.isName())
            {
                oper();
                if (next.isRP())
                {
                    tabs--;
                    pan();
                }
                else
                {
                    err();
                }
            }
            else if (next.isRP())
            {
                tabs--;
                pan();
            }
            else
            {
                err();
            }
            
        }

        private void stmts()
        {
            
            if (next.isLP())
            {
                tabs++;
                pan();
                stmtsP();
                
            }
            else
            {
                err();
            }
            
        }

        private void stmtsP()
        {
            
            if (next.isStdout())
            {
                printstmts();
            }
            else if (next.isWhile())
            {
                whilestmts();
            }
            else if (next.isIf())
            {
                ifstmts();
            }
            else if (next.isLet())
            {
                letstmts();
            }
            else
            {
                err();
            }
            
        }

        private void printstmts()
        {
            
            if (next.isStdout())
            {
                pan();
                oper();
                if (next.isRP())
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

        private void ifstmts()
        {
            
            if (next.isIf())
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

        private void ifstmtsP()
        {
            
            if (next.isRP())
            {
                tabs--;
                pan();
            }
            else if (next.isLP() || next.isConstant() || next.isName())
            {
                expr();
                if (next.isRP())
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

        private void whilestmts()
        {
            
            if (next.isWhile())
            {
                pan();
                expr();
                exprlist();
                if (next.isRP())
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

        private void exprlist()
        {
            
            if (next.isLP() || next.isConstant() || next.isName())
            {
                expr();
                exprlistP();
            }
            else
            {
                err();
            }
            
        }

        private void exprlistP()
        {
            
            if (next.isLP() || next.isConstant() || next.isName())
            {
                exprlist();
            }
            else if (next.isRP())
            {
                //EPSILON
            }
            else
            {
                err();
            }
            
        }

        private void letstmts()
        {
            
            if (next.isLet())
            {
                pan();
                if (next.isLP())
                {
                    varlist();
                    if (next.isRP())
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

        private void varlist()
        {
            
            if (next.isLP())
            {
                pan();
                tabs++;
                if (next.isName())
                {
                    pan();
                    if (next.isType())
                    {
                        pan();
                        if (next.isRP())
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

        private void varlistP()
        {
            if (next.isLP())
            {
                varlist();
            }
            else if (next.isRP())
            {
                //EPSILON
            }
            else
            {
                err();
            }
        }

        private void err()
        {
            Console.WriteLine("Error: unexpected token '{0}' on line {1}.", next.word, lex.getLine());
            Console.ReadLine();
            Environment.Exit(1);
        }

        private void pan()
        {
            for (int i = 0; i < tabs; i++)
            {
                Console.Write("  ");
            }
            Console.WriteLine(next.word);
            next = lex.getNextToken();
        }

        public Node returnTree()
        {
            return root;
        }

    }
}
