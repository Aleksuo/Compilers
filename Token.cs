using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    public enum TokenType
    {
        PLUS, MINUS, MULT, DIV, INTEGER,
        EOF, ERROR, LEFTPAREN, RIGHTPAREN,
        AND, OR, NOT, EQUALS, COLON, SEMICOLON, ID, FOR,
        IN, DO, ASSIGN, RANGE, VAR, END, READ, 
        PRINT, INT, STRING, BOOL, ASSERT, BOOLEAN, 
        STR
    }

    class Token
    {
        private TokenType type;
        private string lexeme;

        public Token(TokenType type, string lexeme)
        {
            this.type = type;
            this.lexeme = lexeme;
        }

        public TokenType getType()
        {
            return this.type;
        }

        public string getLexeme()
        {
            return this.lexeme;
        }

        public string toString()
        {
            return "( " + this.type.ToString() + ", " + this.lexeme + " )";
        }
    }
}
