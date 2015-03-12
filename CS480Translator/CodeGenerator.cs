﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CS480Translator {

    //Type enums for function return types.
    public enum type { boolT, intT, realT, stringT, voidT }; 

    class CodeGenerator {

        //Root of the tree generated by the parserl
        private Tree.NonTerm root;

        //StringBuilder for appending code to.
        private StringBuilder code;

        //Symbol table from parser
        //private SymbolTable st;
        private List<List<Tokens.IT>> scopeLists;
        private int scopeCount;

        //Counters for if and while functions, as well as global variables.
        private int v;

        private int line;
        private int character;

        public CodeGenerator(String file) {

            Parser parser = new Parser(file);
            root = parser.returnTree();
            //st = parser.returnST();

            scopeLists = new List<List<Tokens.IT>>();
            scopeLists.Add(new List<Tokens.IT>());
            scopeCount = 0;

            code = new StringBuilder();

            v = 0;
            line = 0;
            character = 0;

            init();
            start(root);

            code.Append("; x\n");
            code.Append("bye");
        }

        //Adds a new scope list. Increments scope counter.
        private void increaseScope() {
            scopeLists.Add(new List<Tokens.IT>());
            scopeCount++;
        }

        //Removes the current scope. Decrements scope counter.
        private void decreaseScope() {
            scopeLists.Remove(scopeLists[scopeCount]);
            scopeCount--;
        }

        //Attempts to look up variable in a the scope list and its parent scopes.
        private Tokens.IT lookup(Tokens.IT id) {
            for (int i = scopeCount; i >= 0; i--) {
                foreach (Tokens.IT x in scopeLists[i]) {
                    if (id.word == x.word) {
                        return x;
                    }
                }
            }

            return null;
        }

        //Add library functions.
        private void init() {
            code.Append(": boolType if s\" true\" else s\" false\" endif type ;\n");
            code.Append(": n s\\\" \\n\" type ;\n");
            code.Append(": x\n");
        }

        //Starts generating code by looping through every non-terminal. 
        //If it's a term, it's guaranteed to be an ID or constant.
        private void start(Tree.NonTerm root) {

            foreach (Tree.IParseTree node in root.getList()) {

                if (node is Tree.NonTerm && root.getList().Count == 1) {
                    increaseScope();
                    start(node as Tree.NonTerm);
                    decreaseScope();
                }
                if (node is Tree.NonTerm) {
                    increaseScope();
                    unwrapSelect(node as Tree.NonTerm);
                    decreaseScope();
                }
                else {
                    Tree.Term nodeTerm = node as Tree.Term;
                    Tokens.GenericToken token = nodeTerm.getData();

                    if (token is Tokens.BCT || token is Tokens.ICT || token is Tokens.RCT || token is Tokens.SCT) {
                        constant(token);
                    }
                    else {
                        throw new Exception("GC Error: ID values not currently handled on line "
                            + nodeTerm.getLine() + ", character " + nodeTerm.getCharacter() + ".");
                    }
                }
            }
        }

        //Unwraps nested non-terminals, calling func when a terminal is found.
        private type unwrapSelect(Tree.NonTerm node) {

            if (node.getList().Count == 0) {
                return type.voidT;
            }
            else {
                Tree.IParseTree first = node.remove();
                if (first is Tree.NonTerm) {
                    increaseScope();
                    type temp = unwrapSelect(first as Tree.NonTerm);
                    decreaseScope();
                    return temp;
                }
                else {
                    return func(first as Tree.Term, node);
                }
            }
        }

        //Selects function based on first operator.
        private type func(Tree.Term op, Tree.NonTerm pars) {

            Tokens.GenericToken token = op.getData();
            line = op.getLine();
            character = op.getCharacter();
            
            //First operator is a constant.
            if (token is Tokens.BCT || token is Tokens.ICT || token is Tokens.RCT || token is Tokens.SCT) {
                if (pars.getList().Count == 0) {
                    return constant(token);
                }
                else {
                    //We have an identical situation to start where the terms can only be IDs or constants.
                    //Print the constant and give the rest of the pars to start to deal with.
                    constant(token);
                    start(pars);
                    return type.voidT;
                }
            }
            else if (token is Tokens.BOT) {
                return boolOP(token as Tokens.BOT, pars);
            }
            else if (token is Tokens.RMOT) {
                return realMathOP(token as Tokens.RMOT, pars);
            }
            else if (token is Tokens.ROT) {
                return relOP(token as Tokens.ROT, pars);
            }
            else if (token is Tokens.MOT) {
                return mathOP(token as Tokens.MOT, pars);
            }
            else if (token is Tokens.CSOP) {
                return contextOP(token as Tokens.CSOP, pars);
            }
            else if (token is Tokens.KT) {
                return keywordOP(token as Tokens.KT, pars);
            }
            else {
                throw new Exception("GC Error: invalid token in function selector on line " 
                    + line + ", character " + character + "." );
            }
        }

        //Basic invalid return type error function.
        private type err(Tokens.GenericToken op) {

            throw new Exception("GC Error: invalid parameter type in '" + op.word + "' function on line "
                    + line + ", character " + character + ".");

        }

        //Keyword operator, parameters depends on the function.
        private type keywordOP(Tokens.KT op, Tree.NonTerm pars) {

            if (op.word == "stdout") {

                type first = getType(pars);
                if (first == type.stringT) {
                    code.Append("type n\n");
                }
                else if (first == type.intT) {
                    code.Append(". n\n");
                }
                else if (first == type.realT) {
                    code.Append("f. n\n");
                }
                else if (first == type.boolT) {
                    code.Append("boolType n\n");
                }
                else {
                    err(op);
                }
            }
            else if (op.word == "if") {

                type first = getType(pars);

                if (first == type.boolT) {

                    //If only structure
                    if (pars.getList().Count == 1) {

                        ifHelper(op, pars);
                    }
                    //If/Else structure
                    else {

                        ifElseHelper(op, pars);
                    }
                }
                else {
                    err(op);
                }
            }
            else if (op.word == "while") {
                whileHelper(op, pars);
            }
            else if (op.word == "let") {
                letHelper(pars);
            }
            else if (op.word == ":=") {

                Tree.Term id = pars.remove() as Tree.Term;
                Tokens.IT idToken = id.getData() as Tokens.IT;

                idToken = lookup(idToken);

                if (idToken == null) {
                    throw new Exception("GC Error: cannot assign value to undeclared variable on line "
                            + line + ", character " + character + ".");
                }

                type second = getType(pars);

				if (idToken.idType == second) {

					if (idToken.assigned) {
						code.Append("to " + idToken.codeId + "\n");
					}
					else {
						code.Append("{ ");

						if (idToken.idType == type.intT) {
							code.Append(idToken.codeId);
						}
						else if (idToken.idType == type.realT) {
							code.Append("F: " + idToken.codeId);
						}
						else if (idToken.idType == type.boolT) {
							code.Append(idToken.codeId);
						}
						else {
							code.Append("D: " + idToken.codeId);
						}

						code.Append(" }\n");
					}
				}
				else {
					throw new Exception("GC Error: assignment type mismatch on line "
							+ line + ", character " + character + ".");
				}


                idToken.assigned = true;

            }
            else {
                throw new Exception("GC Error: invalid keyword token in keyword function on line "
                        + line + ", character " + character + ".");
            }

            return type.voidT;

        }

        //Parses through let statement assignments
        private void letHelper(Tree.NonTerm vars) {

            vars = vars.remove() as Tree.NonTerm;

            foreach (Tree.IParseTree var in vars.getList()) {

                //Unwrap values. Guaranteed by parser.
                Tree.NonTerm nameTypePair = var as Tree.NonTerm;
                Tree.Term name = nameTypePair.remove() as Tree.Term;
                Tree.Term variableType = nameTypePair.remove() as Tree.Term;

                Tokens.IT nameToken = name.getData() as Tokens.IT;
                Tokens.VTT variableTypeToken = variableType.getData() as Tokens.VTT;

                foreach (Tokens.IT x in scopeLists[scopeCount - 1]) {
                    if (nameToken.word == x.word) {
                        throw new Exception("GC Error: previously initialized variable in the same scope being reinitialized on line "
                                + line + ", character " + character + ".");
                    }
                }

                nameToken.codeId = "v" + v;

                if (variableTypeToken.word == "int") {

                    nameToken.idType = type.intT;
                    //code.Append("variable " + nameToken.codeId + "\n");
                }
                else if (variableTypeToken.word == "real") {

                    nameToken.idType = type.realT;
                    //code.Append("fvariable " + nameToken.codeId + "\n");
                }
                else if (variableTypeToken.word == "bool") {

                    nameToken.idType = type.boolT;
                    //code.Append("variable " + nameToken.codeId + "\n");
                }
                // else string type
                else {

                    nameToken.idType = type.stringT;
                    //code.Append("stringVar " + nameToken.codeId + "\n");
                }

                scopeLists[scopeCount - 1].Add(nameToken);
                v++;
            }
        }

        //Outputs basic loop structure without taking definitions into account.
        private void whileHelper(Tokens.KT op, Tree.NonTerm pars) {

            code.Append("begin\n");

            type first = getType(pars);
            if (!(first == type.boolT)) {
                err(op);
            }

            code.Append("while\n");

            while (pars.getList().Count != 0) {
                getType(pars);
            }

            code.Append("repeat\n");

        }

        //Outputs basic if structure without taking definitions into account.
        private void ifHelper(Tokens.KT op, Tree.NonTerm pars) {

            code.Append("if\n");
            getType(pars);
            code.Append("endif\n");
        }

        //Outputs basic if/else structure without taking definitions into account
        private void ifElseHelper(Tokens.KT op, Tree.NonTerm pars) {

            code.Append("if\n");
            getType(pars);
            code.Append("else\n");
            getType(pars);
            code.Append("endif\n");

        }

        //Context sensitive operator, depends on input and number of parameters.
        private type contextOP(Tokens.CSOP op, Tree.NonTerm pars) {

            if (op.word == "+") {

                type first = getType(pars);
                type second = getType(pars);

                if ((first == type.stringT) && (second == type.stringT)) {
                    code.Append("s+\n");
                    return type.stringT;
                }
                else if ((first == type.intT) && (second == type.realT)) {
                    code.Append("s>f\n");
                }
                else if ((first == type.realT) && (second == type.intT)) {
                    code.Append("s>f\n");
                }
                else if ((first == type.intT) && (second == type.intT)) {
                    code.Append("+\n");
                    return type.intT;
                }
                else if ((first == type.realT) && (second == type.realT)) {
                    //no converting or swapping needed.
                }
                else {
                    return err(op);
                }

                code.Append("f+\n");
                return type.realT;

            }
            else {
                if (pars.getList().Count == 1) {

                    type first = getType(pars);

                    if (first == type.intT) {
                        code.Append("negate\n");
                        return type.intT;
                    }
                    else if (first == type.realT) {
                        code.Append("fnegate\n");
                        return type.realT;
                    }
                    else {
                        return err(op);
                    }
                }
                else {

                    type first = getType(pars);
                    type second = getType(pars);

                    if ((first == type.intT) && (second == type.realT)) {
                        code.Append("s>f\n");
                        code.Append("fswap\n");  
                    }
                    else if ((first == type.realT) && (second == type.intT)) {
                        code.Append("s>f\n");
                    }
                    else if ((first == type.intT) && (second == type.intT)) {
                        code.Append("-\n");
                        return type.intT;
                    }
                    else if ((first == type.realT) && (second == type.realT)) {
                        //no converting or swapping needed.
                    }
                    else {
                        return err(op);
                    }

                    code.Append("f-\n");
                    return type.realT;

                }
            }
        }

        //Math operations that take two parameters
        private type mathOP(Tokens.MOT op, Tree.NonTerm pars) {

            type first = getType(pars);
            type second = getType(pars);

            //Adjust and convert parameters as necessary
            if ((first == type.intT) && (second == type.realT)) {
                code.Append("s>f\n");
                code.Append("fswap\n");            
            }
            else if ((first == type.realT) && (second == type.intT)) {
                code.Append("s>f\n");
            }
            //Perform integer operations if both parameters are ints.
            else if ((first == type.intT) && (second == type.intT)) {

                if (op.word == "*") {
                    code.Append("*\n");
                }
                else if (op.word == "/") {
                    code.Append("/\n");
                }
                else if (op.word == "%") {
                    code.Append("mod\n");
                }
                else {
                    code.Append("s>f\n");
                    code.Append("s>f\n");
                    code.Append("fswap\n");
                    code.Append("f**\n");
                    code.Append("f>s\n");
                }

                return type.intT;
            }
            else if ((first == type.realT) && (second == type.realT)) {
                //no converting or swapping needed.
            }
            else {
                err(op);
            }

            //Real parameters, perform floating point operation.
            if (op.word == "*") {
                code.Append("f*\n");
            }
            else if (op.word == "/") {
                code.Append("f/\n");
            }
            else if (op.word == "%") {
                throw new Exception("GC Error: mod operation requires two integer parameters on line "
                        + line + ", character " + character + ".");
            }
            else {
                code.Append("f**\n");
            }

            return type.realT;
        }

        //Relational operator, takes in two parameters.
        private type relOP(Tokens.ROT op, Tree.NonTerm pars) {

            type first = getType(pars);
            type second = getType(pars);
            string opGforth;

            //Convert IBTL operator word to Gforth word
            if (op.word == "!=") {
                opGforth = "<>";
            }
            else {
                opGforth = op.word;
            }

            if ((first == type.intT) && (second == type.realT)) {
                code.Append("s>f\n");
                code.Append("f" + opGforth + "\n");
            }
            else if ((first == type.realT) && (second == type.intT)) {
                code.Append("s>f\n");
                code.Append("f" + opGforth + "\n");
            }
            else if ((first == type.intT) && (second == type.intT)) {
                code.Append(opGforth + "\n");
            }
            else if ((first == type.realT) && (second == type.realT)) {
                code.Append("f" + opGforth + "\n");
            }
            else {
                err(op);
            }

            return type.boolT;
        }

        //Real math operation, takes in one parameter.
        private type realMathOP(Tokens.RMOT op, Tree.NonTerm pars) {

            type first = getType(pars);
            if (first == type.intT) {
                code.Append("s>f\n");
            }
            else if (first != type.realT) {
                err(op);
            }

            code.Append("f" + op.word + "\n");

            return type.realT;
        }

        //Boolean operation, number of params guaranteed by grammar.
        private type boolOP(Tokens.BOT op, Tree.NonTerm pars) {

            //'not' function with one param.
            if (op.word == "not") {
                type first = getType(pars);

                if (first == type.boolT) {
                    code.Append("invert\n");
                }
                else {
                    err(op);
                }

            }
            //'and' or 'or' function with two params
            else {
                type first = getType(pars);
                type second = getType(pars);

                if ((first == type.boolT) && (second == type.boolT)) {
                    code.Append(op.word + "\n");
                }
                else {
                    err(op);
                }
            }

            return type.boolT;
        }

        //Get the first parameter and unwrap on the non-term or constant on term.
        private type getType(Tree.NonTerm pars) {

            type first;
            Tree.IParseTree firstPar = pars.remove();

            if (firstPar is Tree.Term) {
                Tree.Term firstParTerm = firstPar as Tree.Term;
                first = constant(firstParTerm.getData());
            }
            else {
                Tree.NonTerm firstParNon = firstPar as Tree.NonTerm;
                increaseScope();
                first = unwrapSelect(firstParNon);
                decreaseScope();
            }

            return first;
        }

        //Token given is a constant, append it to the code and return its type.
        private type constant(Tokens.GenericToken token) {

            if (token is Tokens.BCT) {
                code.Append(token.word + "\n");
                return type.boolT;
            }
            else if (token is Tokens.ICT) {
                Tokens.ICT temp = (Tokens.ICT)token;
                code.Append(temp.integerValue + "\n");
                return type.intT;
            }
            else if (token is Tokens.RCT) {
                Tokens.RCT temp = (Tokens.RCT)token;
                code.Append(temp.realValue + "e\n");
                return type.realT;
            }
            else if (token is Tokens.SCT) {
                code.Append("s\" " + token.word + "\"\n");
                return type.stringT;
            }
            else if (token is Tokens.IT) {

                Tokens.IT id = lookup(token as Tokens.IT);
                if (id == null) {
                    throw new Exception("GC Error: id used before it has been declared with a type on line "
                            + line + ", character " + character + ".");
                }

                if (id.assigned) {
                    code.Append(id.codeId + "\n");
                }
                else {
                    throw new Exception("GC Error: id used before it has been assigned a value on line "
                            + line + ", character " + character + ".");
                }

                return id.idType;

            }
            else {
                throw new Exception("GC Error: invalid token in constant function on line "
                        + line + ", character " + character + ".");
            }

        }

        //Return generated code;
        public string getCode() {
            return code.ToString();
        }
    }
}
