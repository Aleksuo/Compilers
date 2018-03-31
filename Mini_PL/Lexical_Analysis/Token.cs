using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL.Lexical_Analysis
{
    public class Token
    {
        private TokenType type;
        private string lexeme;

        public int row;
        public int col;
       

        public Token(TokenType type, string lexeme, int row, int col)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.row = row;
            this.col = col;
        }

        public Token(TokenType type, string lexeme)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.row = 0;
            this.col = 0;
        }

        public TokenType getType()
        {
            return this.type;
        }

        public string getLexeme()
        {
            return this.lexeme;
        }

        public string rowAndCol()
        {
            return "(" + this.row + "," + this.col + ")";
        }

        

        public string toString()
        {
            return "( " + this.type.ToString() + ", " + this.lexeme + " )";
        }
    }
}
