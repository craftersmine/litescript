using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using craftersmine.LiteScript.Cfg.Console;
using craftersmine.LiteScript.Locale.Console;
using craftersmine.Config;

namespace craftersmine.LiteScript.Parser
{
    public class ParserCore
    {
        public delegate void ParsingRunningEventDelegate(object sender, ParsingRunningEventArgs e);

        public event ParsingRunningEventDelegate ParsingRunningEvent;

        private string Filename { get; }

        private string[] _scriptContents;

        public static Commons Settings = SettingsInitializer.Load();
        public static Localization Locale = LocaleLoader.Load();

        public ParserCore(string file)
        {
            
            Filename = file;
            _scriptContents = new Script.Project(file).GetScriptFile().FileContents; // Получаем построчное содержание файла
            Run();
        }

        public void Run()
        {
            if (ParsingRunningEvent != null)
            {
                #region ParsingEventRunning Init
                
                ConsoleWindow.WriteLine(Locale.StateRunning); 
                ParsingRunningEventArgs _prea = new ParsingRunningEventArgs();
                _prea.CurrentCommand = _scriptContents[0];
                ParsingRunningEvent(null, _prea);
                #endregion


            }
        }
    }

    public sealed class ConsoleWindow
    {
        public static void WriteLine(object contents)
        {
            Console.WriteLine(contents);
        }
    }

    public class ParsingRunningEventArgs : EventArgs
    {
        public string CurrentCommand;
    }
}
