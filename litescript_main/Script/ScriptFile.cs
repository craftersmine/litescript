using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace craftersmine.LiteScript.Script
{
    public class ScriptFile
    {
        public string[] FileContents { get; set; }
        public string Filename { get; }

        public ScriptFile(string file)
        {
            Filename = file;

            List<string> _fcontents = new List<string>();

            if (!File.Exists(file))        // Проверяем на существование file
            {
                File.WriteAllText(file, "~lscript\r\n\r\n~end");  // Генерируем если не существует
                foreach (var line in File.ReadAllLines(file))
                {
                    if (!line.StartsWith("#"))
                        _fcontents.Add(line);
                }
                FileContents = _fcontents.ToArray();
            }
            else
                foreach (var line in File.ReadAllLines(file))
                {
                    if (!line.StartsWith("#"))
                        _fcontents.Add(line);
                }
            FileContents = _fcontents.ToArray();  // Иначе читаем в FileContents
        }

        public string[] ReRead()
        {
            FileContents = File.ReadAllLines(Filename);
            return File.ReadAllLines(Filename);
        }

        public void Save(string[] contents)
        {
            File.WriteAllLines(Filename, contents);
        }
    }
}
