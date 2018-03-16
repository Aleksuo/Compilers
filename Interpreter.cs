using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    class Interpreter : NodeVisitor
    {
        private Parser parser;
        public Interpreter(Parser parser)
        {
            this.parser = parser;
        }    

        public int  visit_numNode(AST node){
            return Int32.Parse(node.token.getLexeme());
        }

        public int visit_unaryOpNode(AST node){
            TokenType type = node.token.getType();
            if(type == TokenType.PLUS){
                return +this.visit(node.left);
            }else if(type == TokenType.MINUS){
                return -this.visit(node.left);
            }
            return 0;
        }

        public int visit_opNode(AST node){
            TokenType type = node.token.getType();
            if(type == TokenType.PLUS){
                return this.visit(node.left)+this.visit(node.right);
            }else if(type == TokenType.MINUS){
                return this.visit(node.left)-this.visit(node.right);
            }else if(type == TokenType.MULT){
                return this.visit(node.left)*this.visit(node.right);
            }else if(type == TokenType.DIV){
                return this.visit(node.left)/this.visit(node.right);
            }
            return 0;
        }

        public int run(){
            AST tree = this.parser.parse();
            return this.visit(tree);
        }
    }
}
