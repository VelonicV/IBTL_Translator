using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    //Superclass of tokens that have a defined set of valid inputs, so the validate function is the same for all of them.
    abstract class SetValuesToken : GenericToken
    {
        public string word;
        public string[] validInput;

        protected override bool validate(string value)
        {
            if (!validInput.Contains(value))
            {
                return false;
            }
            else
            {
                word = value;
                return true;
            }
        }

        public override string ToString()
        {
            return (this.GetType().Name + ":\t" + word);
        }

    }
}
