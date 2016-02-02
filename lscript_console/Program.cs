using System;
using craftersmine.LiteScript.Parser;
using craftersmine.LiteScript.Locale.Console;

namespace craftersmine.LiteScript.CustomConsole
{
    class Program
    { 
        static void Main(string[] args)
        {
            Console.Title = "LiteScript";
            ParserCore _pc = new ParserCore(args[0]);

            Localization Locale = LocaleLoader.Load();
            string[] _scriptContents = new Script.ScriptFile(args[0]).FileContents;
            if (_scriptContents[0] == "~lscript" && _scriptContents[_scriptContents.Length - 1] == "~end")
            {
                Console.Title = "LiteScript - " + args[0];
                _pc.ParsingRunningEvent += _pc_ParsingRunningEvent;
                _pc.ParsingStoppedEvent += _pc_ParsingStoppedEvent;
                _pc.Run();
                System.Threading.Thread.Sleep(1000);
                _pc.Parse();
            }
            else
            {
                Console.Title = "LiteScript";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Locale.IncorrectFileError);
                Console.WriteLine(Locale.StateStopped);
                Console.ResetColor();
                Console.ReadKey();
            }

        }

        private static void _pc_ParsingStoppedEvent(object sender, ParsingStoppedEventArgs e)
        {
            if (!e.CanceledByScriptEnd)
            {
                ParserCore.SendError(ParserCore.Locale.StateStoppedWithErr);
            }
            Console.ReadKey();
        }

        private static void _pc_ParsingRunningEvent(object sender, ParsingRunningEventArgs e)
        {
            
        }
    }
}
