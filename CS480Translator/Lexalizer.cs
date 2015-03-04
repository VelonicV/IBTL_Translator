using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CS480Translator
{
    class Lexalizer
    {
        //Maximum buffer size.
        private const int BUFFER_SIZE = 1024;

        //File stream representing the source code.
        private FileStream file;

        //The buffer object, and its associated pointers.
        private byte[] buffer;
        private int buff_pos;
        private int buff_end;

        //Line and character number
        private int line;
        private int character;

        //Attempts to open the file at the given path and initializes the fields.
        public Lexalizer(string filePath)
        {
            file = File.Open(filePath, FileMode.Open, FileAccess.Read);

            buffer = new byte[BUFFER_SIZE];
            buff_pos = 0;
            refillBuffer();

            line = 1;
            character = 0;
        }

        //Return the current line the lexalizer is on.
        public int getLine()
        {
            return line;
        }

        //Return the current character the lexalizer is on.
        public int getCharacter()
        {
            return character;
        }

        //Returns the next token in the file, or null if none are left.
        public Tokens.GenericToken getNextToken()
        {
            while (more())
            {
                if (Char.IsDigit(peek()) || (peek() == '.'))
                {
                    return createNumToken();
                }
                else if (Char.IsLetter(peek()) || (peek() == '_'))
                {
                    return createLetterToken();
                }
                else if (peek() == '"')
                {
                    next();
                    return createStringToken();
                }
                else if ((peek() == '+')) //|| (peek() == '-'))
                {
                    return new Tokens.CSOP(Char.ToString(next()));
                }
                else if (peek() == '-')
                {
                    return createMinusToken();
                }
                else if ((peek() == '*') || (peek() == '/') || (peek() == '%') || (peek() == '^'))
                {
                    return new Tokens.MOT(Char.ToString(next()));
                }
                else if((peek() == '=') || (peek() == '!') || (peek() == '>') || (peek() == '<')) 
                {
                    return createRelOpToken();
                }
                else if ((peek() == '(') || (peek() == ')') || (peek() == ':'))
                {
                    return createSymKeyToken();
                }
                else if(peek() == '\n')
                {
                    next();
                    line++;
                    character = 0;
                }
                else if((peek() == ' ') || (peek() == '\t') || (peek() == '\r'))
                {
                    next();
                }
                else
                {
                    throw new Exception("Error: Invalid syntax starting with '" + peek() 
                                        + "' declared on line " + line + ", character " + character + ".");
                }
            }

            return new Tokens.EOFT("$");
        }

        //Create a minus token or a negative number, depending on context.
        private Tokens.GenericToken createMinusToken()
        {
            next();

            if (Char.IsDigit(peek()) || (peek() == '.'))
            {
                Tokens.GenericToken temp = createNumToken();
                if (temp is Tokens.ICT)
                {
                    return new Tokens.ICT("-" + temp.word);
                }
                else
                {
                    return new Tokens.RCT("-" + temp.word);
                }

            }
            else
            {
                return new Tokens.CSOP("-");
            }
        }

        //Letter-based token parser.
        private Tokens.GenericToken createLetterToken()
        {

            string[] variableTypes = { "int", "real", "bool", "string" };
            string[] booleanOperators = { "and", "or", "not" };
            string[] realMathOperators = { "sin", "cos", "tan" };
            string[] letterKeywords = { "if", "while", "let", "stdout" };
            string[] booleanConstants = { "true", "false" };

            StringBuilder sb = new StringBuilder();

            while ((Char.IsLetter(peek()) || Char.IsNumber(peek()) || (peek() == '_')) && more())
            {
                sb.Append(next());
            }

            string final = sb.ToString();
            if (variableTypes.Contains(final))
            {
                return new Tokens.VTT(final);
            }
            else if (booleanOperators.Contains(final))
            {
                return new Tokens.BOT(final);
            }
            else if (realMathOperators.Contains(final))
            {
                return new Tokens.RMOT(final);
            }
            else if (letterKeywords.Contains(final))
            {
                return new Tokens.KT(final);
            }
            else if (booleanConstants.Contains(final))
            {
                return new Tokens.BCT(final);
            }
            else
            {
                return new Tokens.IT(final);
            }

        }

        //Symbol keyword parser function.
        private Tokens.KT createSymKeyToken()
        {
            StringBuilder sb = new StringBuilder();
            if ((peek() == '(') || (peek() == ')'))
            {
                sb.Append(next());
                return new Tokens.KT(sb.ToString());
            }
            else
            {
                sb.Append(next());
                if ((peek() == '=') && more())
                {
                    sb.Append(next());
                    return new Tokens.KT(sb.ToString());
                }
                else
                {
                    throw new Exception("Error: Invalid syntax starting with ':' declared on line " 
                                       + line + ", character " + character + ".");
                }
            }
        }

        //Relational operator parser function.
        private Tokens.ROT createRelOpToken()
        {
            StringBuilder sb = new StringBuilder();

            if (peek() == '=')
            {
                sb.Append(next());
                return new Tokens.ROT(sb.ToString());
            }
            else if ((peek() == '<') || (peek() == '>'))
            {
                sb.Append(next());
                if ((peek() == '=') && more())
                {
                    sb.Append(next());
                }

                return new Tokens.ROT(sb.ToString());
            }
            else
            {
                sb.Append(next());
                if (peek() == '=')
                {
                    sb.Append(next());
                    return new Tokens.ROT(sb.ToString());
                }
                else
                {
                    throw new Exception("Error: Invalid syntax starting with '!' declared on line " 
                                       + line + ", character " + character + ".");
                }
            }
        }

        //String token parser function.
        private Tokens.SCT createStringToken()
        {
            StringBuilder sb = new StringBuilder();

            while ((peek() != '"') && (peek() != '\n') && more())
            {
                //Instance of a special character.
                if ((peek() == '\\') && more())
                {
                    next();
                    if (peek() == '\\')
                    {
                        next();
                        sb.Append('\\');
                    }
                    else if (peek() == '\'')
                    {
                        next();
                        sb.Append('\'');
                    }
                    else if (peek() == '"')
                    {
                        next();
                        sb.Append('"');
                    }
                    else if (peek() == '?')
                    {
                        next();
                        sb.Append('?');
                    }
                    else if (peek() == 'a')
                    {
                        next();
                        sb.Append('\a');
                    }
                    else if (peek() == 'b')
                    {
                        next();
                        sb.Append('\b');
                    }
                    else if (peek() == 'f')
                    {
                        next();
                        sb.Append('\f');
                    }
                    else if (peek() == 'n')
                    {
                        next();
                        sb.Append('\n');
                    }
                    else if (peek() == 'r')
                    {
                        next();
                        sb.Append('\r');
                    }
                    else if (peek() == 't')
                    {
                        next();
                        sb.Append('\t');
                    }
                    else if (peek() == 'v')
                    {
                        next();
                        sb.Append('\v');
                    }
                    else
                    {
                        throw new Exception("Error: Invalid character escape sequence in string on line " 
                                           + line + ", character " + character + ".");
                    }

                }
                else {
                    sb.Append(next());
                }          
            }

            if ((peek() == '"') && more())
            {
                next();
                return new Tokens.SCT(sb.ToString());
            }
            else
            {
                throw new Exception("Error: End of line reached without finding second pair of quotation marks on line " 
                                   + line + ", character " + character + "."); 
            }

        }

        //Number token parser function.
        private Tokens.GenericToken createNumToken()
        {
            StringBuilder sb = new StringBuilder();

            //Read in all starting integers of the number.
            while (Char.IsDigit(peek()) && more())
            {
                sb.Append(next());
            }

            //Decimal found.
            if ((peek() == '.') && more())
            {
                //Append decimal and the digits following it.
                sb.Append(next());

                while (Char.IsDigit(peek()) && more())
                {
                    sb.Append(next());
                }

                if (!sb.ToString().Any(c => char.IsDigit(c)))
                {
                    throw new Exception("Error: Invalid syntax starting with '.' declared on line " 
                                       + line + ", character " + character + ".");
                }
                //if we find scientific notation, call the appending function.
                else if (((peek() == 'e') || (peek() == 'E')) && more())
                {
                    return scientific(sb);
                }

                //Return the real constant token.
                return new Tokens.RCT(sb.ToString());
            }
            //Scientific notation found, call the appending function.
            else if (((peek() == 'e') || (peek() == 'E')) && more())
            {
                return scientific(sb);
            }
            //Nothing but digits found, return it as an integer token.
            else
            {
                return new Tokens.ICT(sb.ToString());
            }

        }

        //Returns the next character in the file stream.
        private char peek()
        {
            return (char) buffer[buff_pos];
        }

        //Returns the next character in the file stream and increments the position.
        //Refills buffer if necessary.
        private char next()
        {
            char next_char = (char) buffer[buff_pos];
            buff_pos++;
            if (buff_pos > buff_end)
            {
                refillBuffer();
            }

            character++;
            return next_char;
        }

        //Resets the buffer position and refills it.
        private void refillBuffer()
        {
            buff_pos = 0;
            buff_end = file.Read(buffer, 0, BUFFER_SIZE) - 1;
        }

        //True if there's more to read in the buffer.
        private bool more()
        {
            return (buff_end != -1);
        }

        //Appends the digits and sign following the 'e' in a real number.
        private Tokens.RCT scientific(StringBuilder sb)
        {

            sb.Append(next());
            if (((peek() == '-') || (peek() == '+')) && more())
            {
                sb.Append(next());
            }
            if (Char.IsDigit(peek()) && more())
            {
                while (Char.IsDigit(peek()) && more())
                {
                    sb.Append(next());
                }
            }
            else
            {
                throw new Exception("Error: Invalid real constant declared on line " 
                                   + line + ", character " + character + ".");
            }

            return new Tokens.RCT(sb.ToString());
        }

    }
}
