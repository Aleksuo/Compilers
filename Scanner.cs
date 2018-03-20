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

        private Dictionary<string, Token> reserved_keywords;

        public Scanner(string text){
            this.text = text;
            this.pos = 0;
            initKeywords();
        }

        public void initKeywords(){
            reserved_keywords = new Dictionary<string,Token>();

            reserved_keywords["var"] = new Token(TokenType.VAR,"");
            reserved_keywords["for"] = new Token(TokenType.FOR,"");
            reserved_keywords["end"] = new Token(TokenType.END,"");
            reserved_keywords["in"]  = new Token(TokenType.IN,"");
            reserved_keywords["do"]  = new Token(TokenType.DO,"");
            reserved_keywords["read"] = new Token(TokenType.READ, "");
            reserved_keywords["print"] = new Token(TokenType.PRINT, "");
            reserved_keywords["int"] = new Token(TokenType.INT, "");
            reserved_keywords["string"] = new Token(TokenType.STRING, "");
            reserved_keywords["bool"] = new Token(TokenType.BOOL, "");
            reserved_keywords["assert"] = new Token(TokenType.ASSERT, "");        
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
                this.advance();
                number.Append(this.text[this.pos]);
            }
            this.advance();
            return number.ToString();

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

            if(Char.IsLetter(newChar)){
                return this.identifier();
            }

            if(newChar == '"')
            {
                this.advance();
                StringBuilder str = new StringBuilder();
                while(this.text[this.pos] != '"' && this.pos < this.text.Length)
                {
                    str.Append(this.text[this.pos]);
                    this.advance();
                }
                this.advance();
                return new Token(TokenType.STR, str.ToString());
            }

            if(newChar == ';'){
                this.advance();
                return new Token(TokenType.SEMICOLON, newChar.ToString());
            }

            if(newChar == ':'){
                this.advance();
                if(this.text[this.pos] == '='){
                    this.advance();
                    return new Token(TokenType.ASSIGN, ":=");
                }
                return new Token(TokenType.COLON, ":");
            }

            if(newChar == '.'){
                this.advance();
                if(this.text[this.pos] == '.'){
                    this.advance();
                    return new Token(TokenType.RANGE, "..");
                }
            }



            this.advance();
            return new Token(TokenType.ERROR, newChar.ToString());
        }

    }
}
