
namespace CS480Translator.Tokens
{
    //The basic superclass of all token. 
    //Forces all subclasses to validate the input by overriding the validate(GenericToken token) method.
    abstract class GenericToken
    {
        public string word;
        abstract protected bool validate(string value);

    }
}
