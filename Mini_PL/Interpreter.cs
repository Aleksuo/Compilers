using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void visit_printNode(AST node)
        {
            Console.WriteLine(this.visit(node.left).ToString());
        }

        public void visit_readNode(AST node)
        {
            object input = Console.ReadLine();
            this.symbols[node.left.token.getLexeme()] = input;
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
            }
            return null;
        }

        public object run(){
            AST tree = this.parser.parse();
            return this.visit(tree);
        }
    }
}
