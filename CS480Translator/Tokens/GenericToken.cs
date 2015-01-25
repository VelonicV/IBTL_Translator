using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    abstract class GenericToken
    {
        abstract protected bool validate(string value);

    }
}
