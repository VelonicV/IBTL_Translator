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
        private Lexalizer lex;
        private SymbolTable st;
        private Tokens.GenericToken prev;
        private Tokens.GenericToken next;
        private Tree.NonTerm root;
        //int tabs;

        //Initialize class variables
        public Parser(string filePath)
        {
            st = new SymbolTable();
            //tabs = 0;

            lex = new Lexalizer(filePath);
            next = lex.getNextToken();

            root = new Tree.NonTerm(null);

            S(root);

            if (!(next is Tokens.EOFT))
            {
                throw new Exception("Grammatical Error: invalid token '" + next.word + "' following token '" + prev.word + "' found on line " + lex.getLine() + ".");
            }

        }

        // S -> ( S" | constants S' | name S'
        private void S(Tree.NonTerm node)
        {
            if (TokenEquiv.isLP(next))
            {
                pan(node);
                //tabs++;
                Tree.NonTerm temp = new Tree.NonTerm(node);
                node.add(temp);
                SPP(temp);
                
            }
            else if (TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                pan(node);
                SP(node);
            }
            else
            {
                err();
            }
            
        }

        // S' -> S S' | EPSILON
        private void SP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isEOF(next) || TokenEquiv.isRP(next))
            {
                //EPSILON
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                S(node);
                SP(node);
            }
            else
            {
                err();
            }
            
        }

        // S" -> ) S' | S ) S' | expr' S'
        private void SPP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isRP(next))
            {
                //tabs--;
                pan(node.getParent());
                SP(node.getParent());
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                S(node);
                if (TokenEquiv.isRP(next))
                {
                    //tabs--;
                    pan(node.getParent());
                    SP(node.getParent());
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
                exprP(node);
                SP(node);
            }
            else
            {
                err();
            }
            
        }

        // expr -> ( expr' | constants | name
        private void expr(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan(node);
                //tabs++;
                Tree.NonTerm temp = new Tree.NonTerm(node);
                node.add(temp);
                exprP(temp);
                
            }
            else if (TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                pan(node);
            }
            else
            {
                err();
            }
            
        }

        // expr' -> oper' | stmts'
        private void exprP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isAssign(next) || TokenEquiv.isBinary(next) || TokenEquiv.isUnary(next) || TokenEquiv.isMinus(next))
            {
                operP(node);
            }
            else if (TokenEquiv.isStdout(next) || TokenEquiv.isIf(next) || TokenEquiv.isWhile(next) || TokenEquiv.isLet(next))
            {
                stmtsP(node);
            }
            else
            {
                err();
            }
            
        }

        // oper -> ( oper' | constants | name
        private void oper(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan(node);
                //tabs++;
                Tree.NonTerm temp = new Tree.NonTerm(node);
                node.add(temp);
                operP(temp);
                
            }
            else if (TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                pan(node);
            }
            else
            {
                err();
            }
            
        }

        // oper' -> := name oper ) | binops oper oper ) | unops oper ) | - oper oper"
        private void operP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isAssign(next))
            {
                pan(node);
                if (TokenEquiv.isName(next))
                {
                    pan(node);
                    oper(node);
                    if (TokenEquiv.isRP(next))
                    {
                        //tabs--;
                        pan(node.getParent());
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
                pan(node);
                oper(node);
                oper(node);
                if (TokenEquiv.isRP(next))
                {
                    //tabs--;
                    pan(node.getParent());
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isUnary(next))
            {
                pan(node);
                oper(node);
                if (TokenEquiv.isRP(next))
                {
                    //tabs--;
                    pan(node.getParent());
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isMinus(next))
            {
                pan(node);
                oper(node);
                operPP(node);
            }
            else
            {
                err();
            }
            
        }

        // oper" -> oper ) | )
        private void operPP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                oper(node);
                if (TokenEquiv.isRP(next))
                {
                    //tabs--;
                    pan(node.getParent());
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isRP(next))
            {
                //tabs--;
                pan(node.getParent());
            }
            else
            {
                err();
            }
            
        }

        // stmts -> ( stmts'
        private void stmts(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan(node);
                //tabs++;
                Tree.NonTerm temp = new Tree.NonTerm(node);
                node.add(temp);
                stmtsP(temp);
                
            }
            else
            {
                err();
            }
            
        }

        // stmts' -> printstmts | whilestmts | ifstmts | letstmts
        private void stmtsP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isStdout(next))
            {
                printstmts(node);
            }
            else if (TokenEquiv.isWhile(next))
            {
                whilestmts(node);
            }
            else if (TokenEquiv.isIf(next))
            {
                ifstmts(node);
            }
            else if (TokenEquiv.isLet(next))
            {
                letstmts(node);
            }
            else
            {
                err();
            }
            
        }

        // printstmts -> stdout oper )
        private void printstmts(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isStdout(next))
            {
                pan(node);
                oper(node);
                if (TokenEquiv.isRP(next))
                {
                    //tabs--;
                    pan(node.getParent());
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
        private void ifstmts(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isIf(next))
            {
                pan(node);
                expr(node);
                expr(node);
                ifstmtsP(node);
            }
            else 
            {
                err();
            }
            
        }

        // ifstmts' -> ) | expr )
        private void ifstmtsP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isRP(next))
            {
                //tabs--;
                pan(node.getParent());
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                expr(node);
                if (TokenEquiv.isRP(next))
                {
                    //tabs--;
                    pan(node.getParent());
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
        private void whilestmts(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isWhile(next))
            {
                pan(node);
                expr(node);
                exprlist(node);
                if (TokenEquiv.isRP(next))
                {
                    //tabs--;
                    pan(node.getParent());
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
        private void exprlist(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                expr(node);
                exprlistP(node);
            }
            else
            {
                err();
            }
            
        }

        // exprlist' -> exprlist | EPSILON
        private void exprlistP(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) || TokenEquiv.isName(next))
            {
                exprlist(node);
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
        private void letstmts(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLet(next))
            {
                pan(node);
                if (TokenEquiv.isLP(next))
                {
                    varlist(node);
                    if (TokenEquiv.isRP(next))
                    {
                        //tabs--;
                        pan(node.getParent());
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
        private void varlist(Tree.NonTerm node)
        {
            
            if (TokenEquiv.isLP(next))
            {
                pan(node);
                //tabs++;
                Tree.NonTerm temp = new Tree.NonTerm(node);
                node.add(temp);
                if (TokenEquiv.isName(next))
                {
                    pan(temp);
                    if (TokenEquiv.isType(next))
                    {
                        pan(temp);
                        if (TokenEquiv.isRP(next))
                        {
                            //tabs--;
                            pan(temp.getParent());
                            varlistP(temp.getParent());
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
        private void varlistP(Tree.NonTerm node)
        {
            if (TokenEquiv.isLP(next))
            {
                varlist(node);
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

        // Add the token to the parse tree, add it to the symbol table if it's an ID, and get the next token.
        private void pan(Tree.NonTerm node)
        {
            //for (int i = 0; i < tabs; i++)
            //{
            //    Console.Write("  ");
            //}
            //Console.WriteLine(next.word);

            node.add(new Tree.Term(next));

            if (next is Tokens.IT)
            {
                st.add((Tokens.IT)next);
            }

            prev = next;
            next = lex.getNextToken();
        }

        //Return the symbol table;
        public SymbolTable returnST()
        {
            return st;
        }

        public Tree.NonTerm returnTree()
        {
            return root;
        }

    }
}
