using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils.Source;
using Mini_PL.Lexical_Analysis;
using Mini_PL.Semantic_Analysis;
using Mini_PL.Parsing;
using Mini_PL.Interpreting;
using Mini_PL.Utils;
using Mini_PL.Error_Handling;

namespace Mini_PL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Initialize
                FileSource source = new FileSource("Example_Programs/boolean.mpl");
                ErrorManager errorManager = new ErrorManager();
                Scanner scanner = new Scanner(source);
                Parser parser = new Parser(scanner);
                SymbolTableBuildingVisitor symbolTableBuilder = new SymbolTableBuildingVisitor();
                TypeCheckingVisitor typeChecker = new TypeCheckingVisitor(symbolTableBuilder.table);
                Interpreter interpreter = new Interpreter();

                //Hook to error manager
                parser.HookTo(errorManager);
                symbolTableBuilder.HookTo(errorManager);
                typeChecker.HookTo(errorManager);

                AST program = parser.parse();
                symbolTableBuilder.build(program);
                typeChecker.check(program);
                if (!errorManager.areErrors())
                {
                    interpreter.run(program);
                }
                else
                {
                    Console.WriteLine("Program couldn't be executed due to errors.");
                }
                
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                return;
            }
            
        }
    }
}
