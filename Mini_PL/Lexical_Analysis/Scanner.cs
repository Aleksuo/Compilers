using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils.Source;

namespace Mini_PL.Lexical_Analysis
{
    public class Scanner
    {
        private ISource source;
        private int lineCount;
        private int colCount;
       

        private Dictionary<string, Token> reserved_keywords;
        private Dictionary<char, Token> singleCharTokens;

        public Scanner(ISource source){
            this.source = source;
            this.lineCount = 1;
            this.colCount = 1;
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

        public void increaseLineCount()
        {
            this.colCount = 0;
            this.lineCount++;
        }

        public void advance()
        {
            this.colCount++;
            this.source.advance();
        }

        public Token integer()
        {
            StringBuilder number = new StringBuilder();
            number.Append(this.source.currentChar());
            bool error = false;
            while (true)
            {
                char? peek = this.source.peekNextChar();
                if (peek == null || !Char.IsLetterOrDigit((char)peek))
                {
                    break;
                }
                if (!Char.IsDigit((char)peek)){
                    error = true;
                }
                    
                this.source.advance();
                number.Append(this.source.currentChar());
            }
            this.source.advance();
            if(number[0]=='0' && number.Length > 1 || error)
            {
                return new Token(TokenType.ERROR, number.ToString());
            }
            return new Token(TokenType.INTEGER, number.ToString());

        }

        public Token identifier(){
            StringBuilder id = new StringBuilder();
            char cur = (char)this.source.currentChar();
            while(Char.IsLetter(cur) 
            || Char.IsDigit(cur)){
                id.Append(cur);
                if(this.source.peekNextChar() == null){
                    this.advance();
                    break;
                }
                this.advance();
                cur = (char)this.source.currentChar();
            }
            if(reserved_keywords.ContainsKey(id.ToString())){
                return reserved_keywords[id.ToString()];
            }
            return new Token(TokenType.ID, id.ToString(),this.lineCount, this.colCount-1);
        }

        public Token range()
        {
            this.advance();
            if (this.source.currentChar() == '.')
            {
                this.advance();
                return new Token(TokenType.RANGE, "..");
            }
            return new Token(TokenType.ERROR, ".");
        }

        public Token colonOrAssign()
        {
            this.advance();
            if (this.source.currentChar() !=null && this.source.currentChar() == '=')
            {
                this.advance();
                return new Token(TokenType.ASSIGN, ":=");
            }
            return new Token(TokenType.COLON, ":");
        }

        public Token stringToken()
        {
            this.advance();
            StringBuilder str = new StringBuilder();
            while (this.source.currentChar() != null && this.source.currentChar() != '"')
            {
                str.Append(this.source.currentChar());
                this.advance();
            }
            this.advance();
            return new Token(TokenType.STRING, str.ToString());
        }

        public bool skipWhiteSpaceAndNewLines()
        {
            char? cur = this.source.currentChar();
            if(cur != null && (cur == ' ' || cur == '\n' || cur == '\r'|| cur == '\t'))
            {
                while (cur != null && (cur == ' ' || cur == '\n' || cur == '\r' || cur == '\t'))
                {
                    if (cur == '\n')
                    {
                        this.increaseLineCount();
                    }
                    this.advance();
                    cur = this.source.currentChar();
                }
                return true;
            }
            return false;
        }

        public bool skipSingleLineComment()
        {
            char? cur = this.source.currentChar();
            char? next = this.source.peekNextChar();
            if(cur == '/' && next == '/')
            {
                while(cur !=null && cur != '\n' && cur != '\r')
                {
                    this.advance();
                    cur = this.source.currentChar();
                }
                return true;
            }
            return false;
        }

        public bool skipMultiLineComment()
        {
            char? cur = this.source.currentChar();
            char? next = this.source.peekNextChar();
            if(cur == '/' && next == '*')
            {
                this.advance();
                this.advance();
                while (true)
                {
                    cur = this.source.currentChar();
                    next = this.source.peekNextChar();
                    if(cur == null || (cur == '*' && next == '/'))
                    {
                        this.advance();
                        this.advance();
                        break;
                    }else if(cur == '/' && next == '*')
                    {
                        skipMultiLineComment();
                    }
                    if(cur == '\n')
                    {
                        this.increaseLineCount();
                    }
                    this.advance();
                }
                return true;
            }
            return false;
        }

        public Token nextToken()
        {
            //scan input
            bool scanning = true;
            while (scanning)
            {
                scanning = false;
                if (skipSingleLineComment())
                    scanning = true;
                if (skipWhiteSpaceAndNewLines())
                    scanning = true;
                if (skipMultiLineComment())
                    scanning = true;
            }

            //recognize next token
            if(this.source.currentChar() != null)
            {
                char newChar = (char)this.source.currentChar();
                if (singleCharTokens.ContainsKey(newChar))
                {
                    this.advance();
                    return singleCharTokens[newChar];
                }
                else if (Char.IsDigit(newChar))
                {
                    return this.integer();
                }
                else if (Char.IsLetter(newChar))
                {
                    return this.identifier();
                }
                else if (newChar == '"')
                {
                    return this.stringToken();
                }
                else if (newChar == ':')
                {
                    return this.colonOrAssign();
                }
                else if (newChar == '.')
                {
                    return this.range();
                }

                this.advance();
                return new Token(TokenType.ERROR, newChar.ToString());
            }
            return new Token(TokenType.EOF, "eof");

        }

        public int getLineCount()
        {
            return this.lineCount;
        }

    }
   
}
