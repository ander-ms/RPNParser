using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RPNParser.RPNParser.Compute("(((3+2)*(5-2))"));
            Console.WriteLine(Math.Sin(1.57)*100+4/-1.5);
            Console.ReadKey();
        }
    }
}
