using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    //The basic superclass of all tokens. 
    //Forces all subclasses to validate the input by overriding the validate() method.
    abstract class GenericToken
    {
        public string word;
        abstract protected bool validate(string value);

    }
}
