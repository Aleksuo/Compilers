using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils;
using Mini_PL.Lexical_Analysis;

namespace Mini_PL
{
    class Interpreter : NodeVisitor
    {
        private Parser parser;
        public Dictionary<string, object> symbols;

        public Interpreter(Parser parser)
        {
            this.parser = parser;
            this.symbols = new Dictionary<string, object>();
        }
        
        public object visit_varNode(AST node)
        {
            string name = node.token.getLexeme();
            object value = this.symbols[name];
            if(value == null)
            {
                Console.WriteLine("Error variable " + name + " has no value.");
            }
            return value;
        }

        
        public object  visit_numNode(AST node){
            return Int32.Parse(node.token.getLexeme());
        }

        public object visit_strNode(AST node)
        {
            return node.token.getLexeme();
        }

        public object visit_boolNode(AST node)
        {
            if(node.token.getLexeme() == "true")
            {
                return true;
            }
            return false;
        }

        public void visit_forNode(AST node)
        {
            AST var = node.children[0];
            AST range = node.children[1];
            AST stmts = node.children[2];
            for(int i = (int)visit(range.left); i <= (int)visit(range.right); i++)
            {
                this.symbols[var.token.getLexeme()] = i;
                this.visit(stmts);
            }
        }

        public void visit_printNode(AST node)
        {
            Console.WriteLine(this.visit(node.left).ToString());
        }

        public void visit_readNode(AST node)
        {
            string input = Console.ReadLine();
            int n;
            bool isNumber = int.TryParse(input, out n);
            if (isNumber)
            {
                this.symbols[node.left.token.getLexeme()] = n;
            }
            else
            {
                this.symbols[node.left.token.getLexeme()] = input;
            }
            
        }

        public void visit_assertNode(AST node)
        {
            Console.WriteLine((bool)this.visit(node.left));
        }

        public void visit_stmtsNode(AST node)
        {
            foreach(AST n in node.children)
            {
                this.visit(n);
            }
        }

        public void visit_assignNode(AST node)
        {
            string name = node.left.token.getLexeme();
            if(node.right != null)
            {
                symbols[name] = this.visit(node.right);
            }
            else
            {
                symbols[name] = null;
            }           
        }

        public object visit_unaryOpNode(AST node){
            TokenType type = node.token.getType();
            if(type == TokenType.PLUS){
                return +(int)this.visit(node.left);
            }else if(type == TokenType.MINUS){
                return -(int)this.visit(node.left);
            }else if(type == TokenType.NOT)
            {
                return !(bool)this.visit(node.left);
            }
            return null;
        }

        public object visit_opNode(AST node){
            TokenType type = node.token.getType();
            if(type == TokenType.PLUS){
                return (int)this.visit(node.left)+(int)this.visit(node.right);
            }else if(type == TokenType.MINUS){
                return (int)this.visit(node.left)-(int)this.visit(node.right);
            }else if(type == TokenType.MULT){
                return (int)this.visit(node.left)*(int)this.visit(node.right);
            }else if(type == TokenType.DIV){
                return (int)this.visit(node.left)/(int)this.visit(node.right);
            }else if(type == TokenType.EQUALS)
            {
                object left = this.visit(node.left);
                object right = this.visit(node.right);
                if(left is int)
                {
                    return ((int)left == (int)right);
                }
                return ((bool)left == (bool)right);
                
            }else if(type == TokenType.LESSTHAN)
            {
                return ((int)this.visit(node.left) < (int)this.visit(node.right));
            }else if(type == TokenType.AND)
            {
                return ((bool)this.visit(node.left) && (bool)(this.visit(node.right)));
            }
            return null;
        }

        public object run(){
            AST tree = this.parser.parse();
            return this.visit(tree);
        }
    }
}
