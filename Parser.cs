﻿using System;
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
            while(this.currentToken.getType() != TokenType.EOF){
                stmts.Add(this.stmt());
                this.eatToken(TokenType.SEMICOLON);
            }
            return new stmtsNode(stmts);
        }

        public AST stmt(){
            Token token = this.currentToken;
            TokenType tokenT = token.getType();
            if(tokenT == TokenType.VAR)
            {
                //<stmt> ::= "var" <var_ident> ":" <type> [ ":=" <expr> ]
                this.eatToken(TokenType.VAR);
                AST ident = this.var_ident();
                this.eatToken(TokenType.COLON);
                tokenT = this.currentToken.getType();
                if(tokenT == TokenType.INT){
                    this.eatToken(TokenType.INT);
                }else if(tokenT == TokenType.STRING){
                    this.eatToken(TokenType.STRING);
                }else if(tokenT == TokenType.BOOL){
                    this.eatToken(TokenType.BOOL);
                }
                token = this.currentToken;
                this.eatToken(TokenType.ASSIGN);
                return new assignNode(ident, this.expr(), token);
            }else if(tokenT == TokenType.ID)
            {
                //<stmt> ::= <var_ident> ":=" <expr>
                AST ident = this.var_ident();
                token = this.currentToken;
                this.eatToken(TokenType.ASSIGN);
                return new assignNode(ident, this.expr(), token);
            }else if(tokenT == TokenType.PRINT)
            {
                //<stmt> ::= "print" <expr>
                this.eatToken(TokenType.PRINT);
                return new printNode(this.expr());
            }else if(tokenT == TokenType.READ)
            {
                //<stmt> ::= "read" <var_ident>
                this.eatToken(TokenType.READ);
                return new readNode(this.var_ident());
            }
            return null;
        }

        public AST var_ident()
        {
            Token cur = this.currentToken;
            this.eatToken(TokenType.ID);
            return new varNode(cur);
        }

        public AST str()
        {
            Token cur = this.currentToken;
            this.eatToken(TokenType.STR);
            return new strNode(cur);
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
            else
            {
                return node;
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
            }else if(tokenT == TokenType.ID)
            {
                //<opnd> ::= <var_ident>
                return this.var_ident();
            }else if(tokenT == TokenType.STR)
            {
                return this.str();
            }
            return new AST(null,null,null);
        }
    }
}
