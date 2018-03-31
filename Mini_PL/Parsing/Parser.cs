using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils;
using Mini_PL.Lexical_Analysis;
using Mini_PL.Error_Handling;
using Mini_PL.Error_Handling.Messages;

namespace Mini_PL.Parsing
{
    class Parser : IHookable
    {
        private Scanner scanner;
        private Token currentToken;

        private bool panic;

        public ErrorHook hook { get ; set ; }

        public Parser(Scanner scanner){
            this.hook = new ErrorHook();
            this.scanner = scanner;
            this.currentToken = this.scanner.nextToken();
            this.panic = false;
        }

        public AST checkStatus(AST node)
        {
            if (panic)
            {
                return new errorNode(null, null);
            }
            return node;
        }

        public void enterPanicMode()
        {
            this.panic = true;
            TokenType cur = this.currentToken.getType();
            while(cur != TokenType.SEMICOLON && cur != TokenType.EOF)
            {
                this.currentToken = this.scanner.nextToken();
                cur = this.currentToken.getType();
            }
        }

        public void eatToken(TokenType expected) {

            TokenType type = this.currentToken.getType();
            if(panic && expected == type)
            {
                this.panic = false;
            }
            if (panic)
                return;

            
            if (type == expected)
            {
                this.currentToken = this.scanner.nextToken();
            }
            else
            {
                if(type == TokenType.ERROR)
                {
                    this.ThrowErrorMessage(new LexicalError(this.currentToken));
                }
                else
                {
                    this.ThrowErrorMessage(new SyntaxError(this.currentToken));
                }
                this.enterPanicMode();
            }
        }

        public AST parse(){
            return this.prog();
        }

        public AST prog(){
            //<prog> ::= <stmts>
            return this.stmts();
        }

        public AST stmts(){
            //<stmts> ::= <stmt> ";" (<stmt> ";")*
            List<AST> stmts = new List<AST>();
            AST node = this.stmt();
            stmts.Add(node);
            this.eatToken(TokenType.SEMICOLON);
            while(this.currentToken.getType() != TokenType.EOF && this.currentToken.getType() != TokenType.END){
                stmts.Add(this.stmt());
                this.eatToken(TokenType.SEMICOLON);
            }
            return new stmtsNode(stmts);
        }

        public AST stmt(){
            Token token = this.currentToken;
            TokenType tokenT = token.getType();
            if (tokenT == TokenType.VAR)
            {
                //<stmt> ::= "var" <var_ident> ":" <type> [ ":=" <expr> ]
                this.eatToken(TokenType.VAR);
                AST ident = this.checkStatus(this.var_ident());
                this.eatToken(TokenType.COLON);
                AST type = this.checkStatus(new typeNode(this.currentToken));
                this.eatToken(TokenType.TYPE);
                ident.left = type;
                token = this.currentToken;
                tokenT = token.getType();
                if (tokenT == TokenType.ASSIGN)
                {
                    this.eatToken(TokenType.ASSIGN);
                    return this.checkStatus(new vardeclNode(ident, this.expr(), token));
                }
                return this.checkStatus(new vardeclNode(ident, null, token));

            } else if(tokenT == TokenType.FOR)
            {
                //<stmt> ::= "for" <var_ident> "in" <expr> ".." <expr> "do" <stmts> "end" "for"
                this.eatToken(TokenType.FOR);
                AST ident = this.checkStatus(this.var_ident());
                this.eatToken(TokenType.IN);
                AST leftExpr = this.checkStatus(this.expr());
                token = this.currentToken;
                this.eatToken(TokenType.RANGE);
                AST rightExpr =this.checkStatus( this.expr());
                AST range = this.checkStatus(new rangeNode(leftExpr, rightExpr, token));
                this.eatToken(TokenType.DO);
                AST stmts = this.checkStatus(this.stmts());
                this.eatToken(TokenType.END);
                this.eatToken(TokenType.FOR);

                List<AST> nodes = new List<AST>();
                nodes.Add(ident);
                nodes.Add(range);
                nodes.Add(stmts);
                return this.checkStatus(new forNode(nodes));
            } else if (tokenT == TokenType.ID)
            {
                //<stmt> ::= <var_ident> ":=" <expr>
                AST ident = this.var_ident();
                token = this.currentToken;
                this.eatToken(TokenType.ASSIGN);
                return this.checkStatus(new assignNode(ident, this.expr(), token));
            } else if (tokenT == TokenType.PRINT)
            {
                //<stmt> ::= "print" <expr>
                this.eatToken(TokenType.PRINT);
                return this.checkStatus(new printNode(this.expr(), token));
            } else if (tokenT == TokenType.READ)
            {
                //<stmt> ::= "read" <var_ident>
                this.eatToken(TokenType.READ);
                return this.checkStatus(new readNode(this.var_ident(), token));
            }else if (tokenT == TokenType.ASSERT)
            {
                this.eatToken(TokenType.ASSERT);
                this.eatToken(TokenType.LEFTPAREN);
                AST expr = new assertNode(this.expr(), token);
                this.eatToken(TokenType.RIGHTPAREN);
                return this.checkStatus(expr);
            }
            return new errorNode(null,null);
        }

        public AST var_ident()
        {
            Token cur = this.currentToken;
            this.eatToken(TokenType.ID);
            return this.checkStatus(new varNode(cur));
        }

        public AST str()
        {
            Token cur = this.currentToken;
            this.eatToken(TokenType.STRING);
            return this.checkStatus(new strNode(cur));
        }

        public AST expr()
        {
            var token = this.currentToken;
            var tokenT = this.currentToken.getType();
            //expr ::= [<unary_op>]<opnd>
            if (tokenT == TokenType.NOT)
            {
                this.eatToken(TokenType.NOT);
                return this.checkStatus(new unaryOpNode(this.opnd(), token));
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
            }else if(tokenT == TokenType.EQUALS)
            {
                this.eatToken(TokenType.EQUALS);
            }else if(tokenT == TokenType.LESSTHAN)
            {
                this.eatToken(TokenType.LESSTHAN);
            }else if(tokenT == TokenType.AND)
            {
                this.eatToken(TokenType.AND);
            }
            else
            {
                return node;
            }
            node = new opNode(node, this.opnd(), token);
            return this.checkStatus(node);
        }

        public AST opnd()
        {
            var tokenT = this.currentToken.getType();
            var token = this.currentToken;
            if(tokenT == TokenType.INTEGER)
            {
                //<opnd> ::= <int>
                this.eatToken(TokenType.INTEGER);
                return this.checkStatus(new numNode(token));
            }else if(tokenT == TokenType.LEFTPAREN)
            {
                //<opnd> ::= "(" expr ")"
                this.eatToken(TokenType.LEFTPAREN);
                AST result = this.expr();
                this.eatToken(TokenType.RIGHTPAREN);
                return this.checkStatus(result);
            }else if(tokenT == TokenType.ID)
            {
                //<opnd> ::= <var_ident>
                return this.checkStatus(this.var_ident());
            }else if(tokenT == TokenType.STRING)
            {
                return this.checkStatus(this.str());
            }
            return new errorNode(null, null);
        }
    }
}
