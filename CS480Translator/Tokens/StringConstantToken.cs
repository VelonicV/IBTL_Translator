using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class StringConstantToken : GenericToken
    {

        public string stringValue;

        public StringConstantToken(string value)
        {
            if (!validate(value))
            {
                throw new Exception("Error: Invalid string constant passed into token");
            }
        }

        protected override bool validate(string value)
        {
            stringValue = value;
            return true;
        }

    }
}
