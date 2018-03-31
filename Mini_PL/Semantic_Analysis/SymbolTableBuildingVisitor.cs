using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Error_Handling;
using Mini_PL.Error_Handling.Messages;
using Mini_PL.Utils;

namespace Mini_PL.Semantic_Analysis
{
    class SymbolTableBuildingVisitor : NodeVisitor, IHookable
    {
        public SymbolTable table { get; set; }

        public ErrorHook hook { get ; set; }

        public SymbolTableBuildingVisitor()
        {
            this.table = new SymbolTable();
            this.hook = new ErrorHook();
        }

        public void visit_stmtsNode(AST node)
        {
            foreach(AST c in node.children)
            {
                this.visit(c);
            }
        }

        public void visit_forNode(AST node)
        {
            foreach(AST c in node.children){
                this.visit(c);
            }
        }

        public void visit_vardeclNode(AST node)
        {

            AST var = node.left;
            AST type = var.left;
            string varName = var.token.getLexeme();
            if(table.lookup(varName) == null)
            {
                Symbol symbol = new Symbol(var.token.getLexeme(), table.lookup(type.token.getLexeme()).type);
                this.table.define(symbol);
            }
            else
            {
                this.ThrowErrorMessage(new DuplicateDeclarationError(var.token));
            }      
            this.visit(node.right);
        }

        public void visit_assignNode(AST node)
        {
            this.visit(node.left);
            this.visit(node.right);
        }

        public void visit_rangeNode(AST node)
        {
            this.visit(node.left);
            this.visit(node.right);
        }

        public void visit_printNode(AST node)
        {
            this.visit(node.left);
        }

        public void visit_readNode(AST node)
        {
            this.visit(node.left);
        }

        public void visit_assertNode(AST node)
        {
            this.visit(node.left);
        }

        public void visit_varNode(AST node)
        {
            Symbol symbol = table.lookup(node.token.getLexeme());
            if (symbol == null)
            {
                this.ThrowErrorMessage(new UndeclaredVariableError(node.token));
            }
        }

        public void visit_opNode(AST node)
        {
            this.visit(node.left);
            this.visit(node.right);
        }

        public void visit_unaryOpNode(AST node)
        {
            this.visit(node.left);
        }

        public void visit_errorNode(AST node)
        {
            return;
        }

        public void build(AST program)
        {
            this.visit(program);
        }

    }
}
