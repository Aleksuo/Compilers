﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = Console.ReadLine();
            Interpreter interpreter = new Interpreter(input);
            Console.WriteLine(interpreter.expr());
            Console.ReadKey();
        }
    }
}
