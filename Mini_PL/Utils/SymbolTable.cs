using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL.Utils
{
    public enum Type
    {
        INTEGER, STRING, BOOLEAN, ERROR
    }
    public class Symbol {
        public string var;
        public Type type;
        public Symbol(string var, Type type)
        {
            this.var = var;
            this.type = type;
        }

        override
        public string ToString()
        {
            return "(VARIABLE: " + var + ",TYPE: " + type.ToString()+")";
        }
    }

    public class SymbolTable
    {
        Dictionary<string, Symbol> symbols;

        public SymbolTable()
        {
            this.symbols = new Dictionary<string, Symbol>();
            this.initTypes();
        }

        public void initTypes()
        {
            this.define(new Symbol("int", Type.INTEGER));
            this.define(new Symbol("string", Type.STRING));
            this.define(new Symbol("bool", Type.BOOLEAN));
        }

        public void define(Symbol symbol)
        {
            //Console.WriteLine(symbol);
            this.symbols[symbol.var] = symbol;
        }

        public Symbol lookup(string name)
        {
            if (symbols.ContainsKey(name))
            {
                return this.symbols[name];
            }
            return null;           
        }
    }
}
