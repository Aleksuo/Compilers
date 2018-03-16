using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Mini_PL_Interpreter
{
    class NodeVisitor
    {
        public NodeVisitor(){}

        public int visit(AST node){
            MethodInfo mi = this.GetType().GetMethod("visit_"+node.GetType().Name);
            return (int)mi.Invoke(this,new object[]{node});
        }
    }
}
