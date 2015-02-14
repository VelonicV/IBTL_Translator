using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    class TokenEquivalence
    {
        /**
         * BELOW LIE THE EQUIVALENCE FUNCTIONS
         * BECAUSE I HAVE NO FORESIGHT
         * AND AM TOO LAZY TO CHANGE MY TOKEN LOGIC
         **/
        public static bool isType(Tokens.GenericToken token)
        {
            if (token is Tokens.VTT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isUnary(Tokens.GenericToken token)
        {
            if (token is Tokens.RMOT)
            {
                return true;
            }
            else if (token is Tokens.CSOP)
            {
                if (token.word == "-")
                {
                    return true;
                }
                return false;
            }
            else if (token is Tokens.BOT)
            {
                if (token.word == "not")
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public static bool isBinary(Tokens.GenericToken token)
        {
            if ((token is Tokens.MOT) || (token is Tokens.ROT) || (token is Tokens.CSOP))
            {
                return true;
            }
            else if (token is Tokens.BOT)
            {
                if ((token.word == "or") || (token.word == "and"))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public static bool isConstant(Tokens.GenericToken token)
        {
            if ((token is Tokens.SCT) || (token is Tokens.ICT) || (token is Tokens.RCT) || (token is Tokens.BCT))
            {
                return true;
            }
            return false;
        }

        public static bool isName(Tokens.GenericToken token)
        {
            if (token is Tokens.IT)
            {
                return true;
            }
            return false;
        }

        public static bool isLP(Tokens.GenericToken token)
        {
            if (token is Tokens.KT)
            {
                if (token.word == "(")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isRP(Tokens.GenericToken token)
        {
            if (token is Tokens.KT)
            {
                if (token.word == ")")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isWhile(Tokens.GenericToken token)
        {
            if (token is Tokens.KT)
            {
                if (token.word == "while")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isIf(Tokens.GenericToken token)
        {
            if (token is Tokens.KT)
            {
                if (token.word == "if")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isStdout(Tokens.GenericToken token)
        {
            if (token is Tokens.KT)
            {
                if (token.word == "stdout")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isLet(Tokens.GenericToken token)
        {
            if (token is Tokens.KT)
            {
                if (token.word == "let")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isAssign(Tokens.GenericToken token)
        {
            if (token is Tokens.KT)
            {
                if (token.word == ":=")
                {
                    return true;
                }
            }
            return false;
        }

        //public static void testEquivalences()
        //{
        //    Tokens.GenericToken[] tokens = {new Tokens.BCT("true"), new Tokens.BCT("false"), new Tokens.BOT("and"), new Tokens.BOT("or"),
        //                                       new Tokens.BOT("not"), new Tokens.CSOP("+"), new Tokens.CSOP("-"), new Tokens.IT("id"),
        //                                       new Tokens.ICT("123"), new Tokens.KT("("), new Tokens.KT(")"), new Tokens.KT("stdout"),
        //                                       new Tokens.KT("if"), new Tokens.KT("while"), new Tokens.KT("let"), new Tokens.KT(":="),
        //                                       new Tokens.MOT("*"), new Tokens.MOT("/"), new Tokens.MOT("%"), new Tokens.MOT("^"),
        //                                       new Tokens.RCT("123.3"), new Tokens.RMOT("sin"), new Tokens.RMOT("cos"), new Tokens.RMOT("tan"),
        //                                       new Tokens.ROT("="), new Tokens.ROT(">"), new Tokens.ROT(">="), new Tokens.ROT("<"),
        //                                       new Tokens.ROT("<="), new Tokens.ROT("!="), new Tokens.SCT("testing"), new Tokens.VTT("bool"), 
        //                                       new Tokens.VTT("int"), new Tokens.VTT("real"), new Tokens.VTT("string") };

        //    foreach (Tokens.GenericToken token in tokens) {

        //        Console.WriteLine(token.word + "\t" + isLet(token));

        //    }

        //}

    }
}
