using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL
{
    public class Scanner
    {
        private int pos;
        private string text;

        private Dictionary<string, Token> reserved_keywords;
        private Dictionary<char, Token> singleCharTokens;

        public Scanner(string text){
            this.text = text;
            this.pos = 0;
            initKeywords();
            initSingleCharTokens();
        }

        public void initKeywords()
        {
            reserved_keywords = new Dictionary<string,Token>();

            reserved_keywords["var"] = new Token(TokenType.VAR,"var");
            reserved_keywords["for"] = new Token(TokenType.FOR,"for");
            reserved_keywords["end"] = new Token(TokenType.END,"end");
            reserved_keywords["in"]  = new Token(TokenType.IN,"in");
            reserved_keywords["do"]  = new Token(TokenType.DO,"do");
            reserved_keywords["read"] = new Token(TokenType.READ, "read");
            reserved_keywords["print"] = new Token(TokenType.PRINT, "print");
            reserved_keywords["int"] = new Token(TokenType.TYPE, "int");
            reserved_keywords["string"] = new Token(TokenType.TYPE, "string");
            reserved_keywords["bool"] = new Token(TokenType.TYPE, "bool");
            reserved_keywords["assert"] = new Token(TokenType.ASSERT, "assert");        
         }

        public void initSingleCharTokens()
        {
            singleCharTokens = new Dictionary<char, Token>();

            singleCharTokens['+'] = new Token(TokenType.PLUS, "+");
            singleCharTokens['-'] = new Token(TokenType.MINUS, "-");
            singleCharTokens['/'] = new Token(TokenType.DIV, "/");
            singleCharTokens['*'] = new Token(TokenType.MULT, "*");
            singleCharTokens['!'] = new Token(TokenType.NOT, "!");
            singleCharTokens['&'] = new Token(TokenType.AND, "&");
            singleCharTokens['<'] = new Token(TokenType.LESSTHAN, "<");
            singleCharTokens['='] = new Token(TokenType.EQUALS, "=");

            singleCharTokens['('] = new Token(TokenType.LEFTPAREN, "(");
            singleCharTokens[')'] = new Token(TokenType.RIGHTPAREN, ")");
            singleCharTokens[';'] = new Token(TokenType.SEMICOLON, ";");

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




        public Token integer()
        {
            StringBuilder number = new StringBuilder();
            number.Append(this.text[this.pos]);
            bool error = false;
            while (true)
            {
                char? peek = this.peekNextChar();
                if (peek == null || !Char.IsLetterOrDigit((char)peek))
                {
                    break;
                }
                if (!Char.IsDigit((char)peek)){
                    error = true;
                }
                    
                this.advance();
                number.Append(this.text[this.pos]);
            }
            this.advance();
            if(number[0]=='0' && number.Length > 1 || error)
            {
                return new Token(TokenType.ERROR, number.ToString());
            }
            return new Token(TokenType.INTEGER, number.ToString());

        }

        public Token identifier(){
            StringBuilder id = new StringBuilder();
            while(Char.IsLetter(this.text[this.pos]) 
            || Char.IsDigit(this.text[this.pos])){
                id.Append(this.text[this.pos]);
                if(this.peekNextChar() == null){
                    this.advance();
                    break;
                }
                this.advance();
            }
            if(reserved_keywords.ContainsKey(id.ToString())){
                return reserved_keywords[id.ToString()];
            }
            return new Token(TokenType.ID, id.ToString());
        }

        public Token range()
        {
            this.advance();
            if (this.text[this.pos] == '.')
            {
                this.advance();
                return new Token(TokenType.RANGE, "..");
            }
            return new Token(TokenType.ERROR, ".");
        }

        public Token colonOrAssign()
        {
            this.advance();
            if (this.pos < this.text.Length && this.text[this.pos] == '=')
            {
                this.advance();
                return new Token(TokenType.ASSIGN, ":=");
            }
            return new Token(TokenType.COLON, ":");
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
            if (singleCharTokens.ContainsKey(newChar))
            {
                this.advance();
                return singleCharTokens[newChar];
            }
           

            if (Char.IsDigit(newChar))
            {
                return this.integer();
            }

            if (Char.IsLetter(newChar)){
                return this.identifier();
            }

            if(newChar == '"')
            {
                this.advance();
                StringBuilder str = new StringBuilder();
                while(this.pos < this.text.Length && this.text[this.pos] != '"')
                {
                    str.Append(this.text[this.pos]);
                    this.advance();
                }
                this.advance();
                return new Token(TokenType.STRING, str.ToString());
            }

            
            if(newChar == ':'){
                return this.colonOrAssign();
            }

            if(newChar == '.'){
                return this.range();
            }



            this.advance();
            return new Token(TokenType.ERROR, newChar.ToString());
        }

    }
}
