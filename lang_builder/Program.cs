using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace lang_builder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = Application.StartupPath + @"\console.lang";
                Localization _locale = new Localization();

                //Build
                _locale.StateRunning = "Выполняется...";
                _locale.StateStopped = "Остановленно!";
                _locale.IncorrectFileError = "Файл не является скриптом! Возможно файл поврежден!";
                _locale.VariableNameCannotBeNull = "Ошибка на строке $linenum! Имя переменной не может быть пустым значением!";
                _locale.StateStoppedWithErr = "Остановлено из-за ошибки в скрипте";
                _locale.VariableNotInitialized = "Ошибка на строке $linenum! Невозможно получить значение переменной $name так как она не инициализирована!";

                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(_locale));
                Console.WriteLine("Success!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }

    public class Localization
    {
        #region States
        public string StateRunning;
        public string StateStopped;
        public string StateStoppedWithErr;
        #endregion

        #region Errors
        public string IncorrectFileError;
        public string VariableNameCannotBeNull;
        public string VariableNotInitialized;
        #endregion
    }
}
