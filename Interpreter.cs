using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    class Interpreter
    {
        public Interpreter(string text)
        {
            this.text = text;
            this.pos = 0;
            this.currentToken = this.nextToken();
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
        
        //Scanner
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


        public void eatToken(TokenType expected)
        {
            if(this.currentToken.getType() == expected)
            {
                this.currentToken = this.nextToken();
            }
            else
            {
                Console.WriteLine("Error during parsing.");
                Console.WriteLine(this.currentToken.toString());
            }
        }

        public string expr()
        {
            var result = this.opnd();
            var tokenT = this.currentToken.getType();
            
            if(tokenT == TokenType.PLUS)
            {
                this.eatToken(TokenType.PLUS);
                result = (Int32.Parse(result) + Int32.Parse(this.opnd())).ToString();
            }else if(tokenT == TokenType.MINUS)
            {
                this.eatToken(TokenType.MINUS);
                result = (Int32.Parse(result) - Int32.Parse(this.opnd())).ToString();
            }else if(tokenT == TokenType.DIV)
            {
                this.eatToken(TokenType.DIV);
                result = (Int32.Parse(result) / Int32.Parse(this.opnd())).ToString();
            }else if(tokenT == TokenType.MULT)
            {
                this.eatToken(TokenType.MULT);
                result = (Int32.Parse(result) * Int32.Parse(this.opnd())).ToString();
            }
            return result;
        }

        public string opnd()
        {
            var tokenT = this.currentToken.getType();
            var token = this.currentToken;
            if(tokenT == TokenType.INTEGER)
            {
                this.eatToken(TokenType.INTEGER);
                return token.getLexeme();
            }else if(tokenT == TokenType.LEFTPAREN)
            {
                this.eatToken(TokenType.LEFTPAREN);
                var result = this.expr();
                this.eatToken(TokenType.RIGHTPAREN);
                return result;
            }
            return "";
        }

       
       

     private char curChar;
     private string text;
     private int pos;
     private Token currentToken;

    }
}
