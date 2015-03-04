using System;

namespace CS480Translator
{
    class Parser
    {
        //Lexalizer object.
        private Lexalizer lex;

        //Symbol table to store ids.
        private SymbolTable st;

        //The previous and next token retrieved from the lexalizer.
        private Tokens.GenericToken prev;
        private Tokens.GenericToken next;

        //The root of the parse tree and the nonterminal parent being worked on.
        private Tree.NonTerm root;
        private Tree.NonTerm node;

        //Initialize class variables
        public Parser(string filePath)
        {
            st = new SymbolTable(null);

            lex = new Lexalizer(filePath);
            next = lex.getNextToken();

            root = new Tree.NonTerm(null);
            node = root;

            S();

            if (!(next is Tokens.EOFT))
            {
                throw new Exception("Error: invalid token '" + next.word + "' following token '" 
                                    + prev.word + "' found on line " + lex.getLine() + ", character " 
                                    + lex.getCharacter() + ".");
            }
        }

        // S -> ( S" | constants S' | name S'
        private void S()
        {
            if (TokenEquiv.isLP(next))
            {
                leftParen();
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
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) 
                    || TokenEquiv.isName(next))
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
                rightParen();
                SP();
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) 
                    || TokenEquiv.isName(next))
            {
                S();
                if (TokenEquiv.isRP(next))
                {
                    rightParen();
                    SP();
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isAssign(next) || TokenEquiv.isBinary(next)
                    || TokenEquiv.isUnary(next) || TokenEquiv.isMinus(next)
                    || TokenEquiv.isStdout(next) || TokenEquiv.isIf(next) 
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
                leftParen();
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
            if (TokenEquiv.isAssign(next) || TokenEquiv.isBinary(next) 
               || TokenEquiv.isUnary(next) || TokenEquiv.isMinus(next))
            {
                operP();
            }
            else if (TokenEquiv.isStdout(next) || TokenEquiv.isIf(next) 
                    || TokenEquiv.isWhile(next) || TokenEquiv.isLet(next))
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
                leftParen();
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
                        rightParen();
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
                    rightParen();
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
                    rightParen();
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
            if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) 
               || TokenEquiv.isName(next))
            {
                oper();
                if (TokenEquiv.isRP(next))
                {
                    rightParen();
                }
                else
                {
                    err();
                }
            }
            else if (TokenEquiv.isRP(next))
            {
                rightParen();
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
                leftParen();
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
                    rightParen();
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
                rightParen();
            }
            else if (TokenEquiv.isLP(next) || TokenEquiv.isConstant(next) 
                    || TokenEquiv.isName(next))
            {
                expr();
                if (TokenEquiv.isRP(next))
                {
                    rightParen();
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
                    rightParen();
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
                    leftParen();
                    varlist();
                    if (TokenEquiv.isRP(next))
                    {
                        rightParen();
                        if (TokenEquiv.isRP(next))
                        {
                            rightParen();          
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

        // varlist -> ( name type ) varlist'
        private void varlist()
        {
            
            if (TokenEquiv.isLP(next))
            {
                leftParen();
                if (TokenEquiv.isName(next))
                {
                    pan();
                    if (TokenEquiv.isType(next))
                    {
                        pan();
                        if (TokenEquiv.isRP(next))
                        {
                            rightParen();
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
                throw new Exception("Error: unexpected end of file reached on line " 
                                    + lex.getLine() + ", character " + lex.getCharacter() + ".");
            }
            else if(prev == null)
            {
                throw new Exception("Error: invalid starting token '" + next.word 
                                    + "' found on line " + lex.getLine() + ", character " 
                                    + lex.getCharacter() + ".");
            }
            else
            {
                throw new Exception("Error: invalid token '" + next.word + "' following token '" 
                                    + prev.word + "' found on line " + lex.getLine() 
                                    + ", character " + lex.getCharacter() + ".");
            }
        }

        // Add the token to the parse tree, add it to the symbol table if it's an ID, and get the next token.
        private void pan()
        {
            node.add(new Tree.Term(next, lex.getLine(), lex.getCharacter()));

            if (next is Tokens.IT)
            {
                st.add((Tokens.IT) next);
            }

            prev = next;
            next = lex.getNextToken();
        }

        //Return the symbol table;
        public SymbolTable returnST()
        {
            return st;
        }

        //Return tree.
        public Tree.NonTerm returnTree()
        {
            return root;
        }

        //Consume a left paren and add a new non-term to work within.
        private void leftParen()
        {
            //pan();
            prev = next;
            next = lex.getNextToken();

            node = new Tree.NonTerm(node);
            node.getParent().add(node);
        }

        //Consume a right parent and adjust the non-terminal being worked on.
        private void rightParen()
        {
            node = node.getParent();

            prev = next;
            next = lex.getNextToken();
            //pan();
        }

        public int getLine() {
            return lex.getLine();
        }

        public int getCharacter() {
            return lex.getCharacter();
        }

    }
}
