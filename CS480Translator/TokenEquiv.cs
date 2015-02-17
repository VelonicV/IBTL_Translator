using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    //Provides static method for checking for grammatical terminal to token equivalence.
    class TokenEquiv
    {
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
            if ((token is Tokens.MOT) || (token is Tokens.ROT))
            {
                return true;
            }
            else if (token is Tokens.CSOP)
            {
                if (token.word == "+")
                {
                    return true;
                }
                return false;
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

        public static bool isMinus(Tokens.GenericToken token)
        {
            if (token is Tokens.CSOP)
            {
                if (token.word == "-")
                {
                    return true;
                }
            }
            return false;
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

        public static bool isEOF(Tokens.GenericToken token)
        {
            if (token is Tokens.EOFT)
            {
                return true;
            }
            return false;
        }

    }
}
