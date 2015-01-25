using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class IntegerConstantToken : GenericToken
    {

        public int integer;

        public IntegerConstantToken(string value)
        {
            if (!validate(value))
            {
                throw new Exception("Error: Invalid integer string passed into token");
            }
        }

        protected override bool validate(string value)
        {
            try
            {
                integer = Convert.ToInt32(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
