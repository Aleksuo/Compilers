using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    class Parser
    {
        private Scanner scanner;
        private Token currentToken;

        public Parser(Scanner scanner){
            this.scanner = scanner;
            this.currentToken = this.scanner.nextToken();
        }

        public AST parse(){
            return this.expr();
        }

        public void eatToken(TokenType expected)
        {
            if(this.currentToken.getType() == expected)
            {
                this.currentToken = this.scanner.nextToken();
            }
            else
            {
                Console.WriteLine("Error during parsing.");
                Console.WriteLine(this.currentToken.toString());
            }
        }

        public AST expr()
        {
            var token = this.currentToken;
            var tokenT = this.currentToken.getType();
            //expr ::= [<unary_op>]<opnd>
            if(tokenT == TokenType.PLUS){
                this.eatToken(TokenType.PLUS);
                return new unaryOpNode(this.opnd(),token);
            }else if(tokenT == TokenType.MINUS){
                this.eatToken(TokenType.MINUS);
                return new unaryOpNode(this.opnd(), token);
            }

            //expr ::= <opnd> <op> <opnd>
            AST node = this.opnd();
            token = this.currentToken;
            tokenT = this.currentToken.getType();
            
            if(tokenT == TokenType.PLUS)
            {
                this.eatToken(TokenType.PLUS);
                
            }else if(tokenT == TokenType.MINUS)
            {
                this.eatToken(TokenType.MINUS);
    
            }else if(tokenT == TokenType.DIV)
            {
                this.eatToken(TokenType.DIV);
        
            }else if(tokenT == TokenType.MULT)
            {
                this.eatToken(TokenType.MULT);
            }
            node = new opNode(node, this.opnd(), token);
            return node;
        }

        public AST opnd()
        {
            var tokenT = this.currentToken.getType();
            var token = this.currentToken;
            if(tokenT == TokenType.INTEGER)
            {
                //<opnd> ::= <int>
                this.eatToken(TokenType.INTEGER);
                return new numNode(token);
            }else if(tokenT == TokenType.LEFTPAREN)
            {
                //<opnd> ::= "(" expr ")"
                this.eatToken(TokenType.LEFTPAREN);
                AST result = this.expr();
                this.eatToken(TokenType.RIGHTPAREN);
                return result;
            }
            return new AST(null,null,null);
        }
    }
}
