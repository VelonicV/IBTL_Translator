using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class BooleanConstantToken : GenericToken
    {
        public bool booleanValue;
        public BooleanConstantToken(string value)
        {
            if (!validate(value))
            {
                throw new Exception("Error: Invalid boolean constant string passed into token");
            }
        }

        protected override bool validate(string value)
        {
            if (value == "true")
            {
                booleanValue = true;
            }
            else if (value == "false")
            {
                booleanValue = false;
            }
            else
            {
                return false;
            }

            return true;
        }

    }
}
