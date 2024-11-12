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
