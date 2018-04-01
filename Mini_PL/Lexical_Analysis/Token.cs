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

        public int line;
       

        public Token(TokenType type, string lexeme, int row)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.line = row;
            
        }

        public Token(TokenType type, string lexeme)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.line = 0;
            
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
