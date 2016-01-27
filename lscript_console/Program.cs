using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using craftersmine.LiteScript.Parser;

namespace craftersmine.LiteScript.CustomConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ParserCore _pc = new ParserCore(@"D:\testproj\test.lsproj");
            _pc.ParsingRunningEvent += _pc_ParsingRunningEvent;
            _pc.Run();
        }

        private static void _pc_ParsingRunningEvent(object sender, ParsingRunningEventArgs e)
        {
            Console.WriteLine("Event Working!");
            Console.ReadKey();
        }
    }
}
