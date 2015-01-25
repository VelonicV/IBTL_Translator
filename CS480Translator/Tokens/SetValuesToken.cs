using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    abstract class SetValuesToken : GenericToken
    {
        public string word;
        protected string[] validInput;

        protected override bool validate(string value)
        {
            if (!validInput.Contains(value))
            {
                return false;
            }

            word = value;
            return true;
        }

    }
}
