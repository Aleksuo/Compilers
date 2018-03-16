using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    class Scanner
    {
        private int pos;
        private string text;

        public Scanner(string text){
            this.text = text;
            this.pos = 0;
        }

         public void advance()
        {
            this.pos++;
        }

        public char? peekNextChar()
        {
            if(this.pos+1 > text.Length - 1)
            {
                return null;
            }
            else
            {
                return text[this.pos + 1];
            }
        }


        public string integer()
        {
            StringBuilder number = new StringBuilder();
            number.Append(this.text[this.pos]);
            while (true)
            {
                char? peek = this.peekNextChar();
                if (peek == null || !Char.IsDigit((char)peek))
                    break;
                this.pos++;
                number.Append(this.text[this.pos]);
            }
            this.advance();
            return number.ToString();

        }

        public Token nextToken()
        {
            while(this.pos < text.Length && this.text[this.pos] == ' ')
            {
                this.advance();
            }

            if(this.pos > text.Length - 1)
            {
                return new Token(TokenType.EOF, "");
            }

            

            char newChar = this.text[this.pos];

            if (newChar == '+')
            {
                this.advance();
                return new Token(TokenType.PLUS, newChar.ToString());
            }

            if(newChar == '-')
            {
                this.advance();
                return new Token(TokenType.MINUS, newChar.ToString());
            }

            if(newChar == '*')
            {
                this.advance();
                return new Token(TokenType.MULT, newChar.ToString());
            }

            if(newChar== '/')
            {
                this.advance();
                return new Token(TokenType.DIV, newChar.ToString());
            }

            if(newChar == '(')
            {
                this.advance();
                return new Token(TokenType.LEFTPAREN, newChar.ToString());
            }

            if(newChar == ')')
            {
                this.advance();
                return new Token(TokenType.RIGHTPAREN, newChar.ToString());
            }

            if (Char.IsDigit(newChar))
            {               
                return new Token(TokenType.INTEGER, this.integer());
            }

            this.advance();
            return new Token(TokenType.ERROR, newChar.ToString());
        }

    }
}
