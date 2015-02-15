using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    //The basic superclass of all thiss. 
    //Forces all subclasses to validate the input by overriding the validate() method.
    abstract class GenericToken
    {
        public string word;
        abstract protected bool validate(string value);

        public bool isType()
        {
            if (this is Tokens.VTT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isUnary()
        {
            if (this is Tokens.RMOT)
            {
                return true;
            }
            else if (this is Tokens.BOT)
            {
                if (this.word == "not")
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

        public bool isBinary()
        {
            if ((this is Tokens.MOT) || (this is Tokens.ROT))
            {
                return true;
            }
            else if (this is Tokens.CSOP)
            {
                if (this.word == "+")
                {
                    return true;
                }
                return false;
            }
            else if (this is Tokens.BOT)
            {
                if ((this.word == "or") || (this.word == "and"))
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

        public bool isMinus()
        {
            if (this is Tokens.CSOP)
            {
                if (this.word == "-")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isConstant()
        {
            if ((this is Tokens.SCT) || (this is Tokens.ICT) || (this is Tokens.RCT) || (this is Tokens.BCT))
            {
                return true;
            }
            return false;
        }

        public bool isName()
        {
            if (this is Tokens.IT)
            {
                return true;
            }
            return false;
        }

        public bool isLP()
        {
            if (this is Tokens.KT)
            {
                if (this.word == "(")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isRP()
        {
            if (this is Tokens.KT)
            {
                if (this.word == ")")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isWhile()
        {
            if (this is Tokens.KT)
            {
                if (this.word == "while")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isIf()
        {
            if (this is Tokens.KT)
            {
                if (this.word == "if")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isStdout()
        {
            if (this is Tokens.KT)
            {
                if (this.word == "stdout")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isLet()
        {
            if (this is Tokens.KT)
            {
                if (this.word == "let")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isAssign()
        {
            if (this is Tokens.KT)
            {
                if (this.word == ":=")
                {
                    return true;
                }
            }
            return false;
        }

    }
}
