using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class RCT : GenericToken
    {

        public double realValue;

        public RCT(string value)
        {
            if (!validate(value))
            {
                throw new Exception("Error: Invalid real constant string passed into token");
            }
        }

        protected override bool validate(string value)
        {
            try
            {
                realValue = Convert.ToDouble(value);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public override string ToString()
        {
            return (this.GetType().Name + ":\t" + realValue);
        }
    }
}
