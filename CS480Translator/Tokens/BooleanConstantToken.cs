using System;

namespace CS480Translator.Tokens
{
    class BCT : GenericToken
    {
        public bool booleanValue;
        private static string trueString = "true";
        private static string falseString = "false";

        public BCT(string value)
        {
            if (!validate(value))
            {
                throw new Exception("Error: Invalid boolean constant string passed into token");
            }
        }

        protected override bool validate(string value)
        {
            if (value == trueString)
            {
                booleanValue = true;
                word = value;
            }
            else if (value == falseString)
            {
                booleanValue = false;
                word = value;
            }
            else
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return (this.GetType().Name + ":\t" + booleanValue);
        }

    }
}
