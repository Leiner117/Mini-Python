using System;
using InstructionsNameSpace;
using AlmacenNameSpace;
using DesensambladorNameSpace;
using moduloPila;
using System.Collections;
using System.Collections.Generic;

namespace Minics.exe
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length == 0) || (args.Length > 1))
            {
                System.Console.WriteLine("Wrong number of arguments.");
                System.Console.WriteLine("Usage: Minics <file.txt>");
            }
            else
            {
                InstructionSet instructionSet = new InstructionSet();
                Desensamblador desensamblador = new Desensamblador(ref instructionSet);
                desensamblador.desensamblar(args[0]);
                List<string> list = instructionSet.run();
                
                Console.WriteLine("Instructions:");
                foreach (string s in list)
                {
                    Console.WriteLine(s);
                }
            }
        }
    }
}
