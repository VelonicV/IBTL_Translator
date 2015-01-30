using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class RCT : GenericToken
    {

        public float realValue;

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
                realValue = Convert.ToSingle(value);
                return true;
            }
            catch (OverflowException e)
            {
                Console.WriteLine("Error: Real constant too large to store in real value type.");
                
                Environment.Exit(1);
            }
            catch { }

            return false;

        }

        public override string ToString()
        {
            return (this.GetType().Name + ":\t" + realValue);
        }
    }
}
