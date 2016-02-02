using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;

using craftersmine.LiteScript.Cfg.Console;
using craftersmine.LiteScript.Locale.Console;

namespace craftersmine.LiteScript.Parser
{
    public class ParserCore
    {
        #region Delegates
        public delegate void ParsingRunningEventDelegate(object sender, ParsingRunningEventArgs e);
        public delegate void ParsingStoppedEventDelegate(object sender, ParsingStoppedEventArgs e);
        #endregion

        #region Events
        public event ParsingRunningEventDelegate ParsingRunningEvent;
        public event ParsingStoppedEventDelegate ParsingStoppedEvent;
        #endregion

        #region Properties
        private string Filename { get; }
        private int CommentLinesCounter { get; }
        #endregion

        #region Fields and Arrays
        private string[] _scriptContents;     // Script without commented lines

        public static Commons Settings = SettingsInitializer.Load();       // Settings
        public static Localization Locale = LocaleLoader.Load();           // Localizaton
        #endregion

        public ParserCore(string file)
        {
            
            Filename = file;
            _scriptContents = new Script.ScriptFile(Filename).FileContents; // Получаем построчное содержание файла
            CommentLinesCounter = new Script.ScriptFile(Filename).CommentLinesCount;
            Run();
        }

        public void Run()
        {
            if (ParsingRunningEvent != null)
            {
                #region ParsingEventRunning Init

                Console.WriteLine(Locale.StateRunning + "\r\n");
                ParsingRunningEventArgs _prea = new ParsingRunningEventArgs();
                _prea.IsRunning = true;
                ParsingRunningEvent(null, _prea);
                #endregion

            }
        }

        public void Parse()
        {
            bool stopReasonIsEnd = true;

            #region Variable DB
            Dictionary<string, string> _stringVars = new Dictionary<string, string>();
            Dictionary<string, int>    _intVars    = new Dictionary<string, int>();
            #endregion

            #region Regex Patterns
            //system
            string pattern_system_out_write = @"system:out:write\[""(.*)""\]";
            string pattern_system_out_writeLn = @"system:out:write@Ln\[""(.*)""\]";
            string pattern_system_setTitle = @"system:setTitle\[""(.*)""\]";

            string pattern_system_out_write_var = @"system:out:write\[\$([\w\s].*)=>getValue\]";
            string pattern_system_out_writeLn_var = @"system:out:write@Ln\[\$([\w\s].*)=>getValue\]";

            string pattern_system_in_read = @"system:in:read\[\$(.*)<=setValue\]";
            //variables
            string pattern_string_var = @"string\$(.*)<=setValue\[""(.*)""\]";
            #endregion

            int _lineCounter = 1 + CommentLinesCounter;

            foreach (var line in _scriptContents)
            {
                #region system:
                //system:out:write
                //normaltext
                if (Regex.IsMatch(line, pattern_system_out_write))
                    Console.Write(Regex.Match(line, pattern_system_out_write).Groups[1].Value);


                //variable val
                if (Regex.IsMatch(line, pattern_system_out_write_var))
                {
                    if (_stringVars.ContainsKey(Regex.Match(line, pattern_system_out_write_var).Groups[1].Value))
                    {
                        string val;
                        _stringVars.TryGetValue(Regex.Match(line, pattern_system_out_write_var).Groups[1].Value, out val);
                        Console.Write(val);
                    }
                    else { SendError(Locale.VariableNotInitialized.Replace("$name", Regex.Match(line, pattern_system_out_write_var).Groups[1].Value).Replace("$linenum", _lineCounter.ToString())); stopReasonIsEnd = false; break; }
                }


                //system:out:write@Ln
                //normaltext
                if (Regex.IsMatch(line, pattern_system_out_writeLn))
                {
                    string capture = Regex.Match(line, pattern_system_out_writeLn).Groups[1].Value;
                    if (capture == string.Empty)
                        Console.WriteLine();
                    else
                        Console.WriteLine(capture);
                }


                //variable val
                if (Regex.IsMatch(line, pattern_system_out_writeLn_var))
                {
                    if (_stringVars.ContainsKey(Regex.Match(line, pattern_system_out_writeLn_var).Groups[1].Value))
                    {
                        string val;
                        _stringVars.TryGetValue(Regex.Match(line, pattern_system_out_writeLn_var).Groups[1].Value, out val);
                        if (val == "null")
                            Console.WriteLine("\r\n");
                        else
                            Console.WriteLine(val);
                    }
                    else { SendError(Locale.VariableNotInitialized.Replace("$name", Regex.Match(line, pattern_system_out_writeLn_var).Groups[1].Value).Replace("$linenum", _lineCounter.ToString())); stopReasonIsEnd = false; break; }


                }


                //system:setTitle
                if (Regex.IsMatch(line, pattern_system_setTitle))
                    Console.Title = Regex.Match(line, pattern_system_setTitle).Groups[1].Value;


                //system:in:read
                if (Regex.IsMatch(line, pattern_system_in_read))
                {
                    string varNameCapture = Regex.Match(line, pattern_system_in_read).Groups[1].Value;
                    Console.WriteLine();
                    Console.Write("Input > ");
                    string readed = Console.ReadLine().Replace("Input > ", "");
                    if (_stringVars.ContainsKey(varNameCapture))
                    {
                        _stringVars.Remove(varNameCapture);
                        _stringVars.Add(varNameCapture, readed);
                    }
                    else { SendError(Locale.VariableNotInitialized.Replace("$name", Regex.Match(line, pattern_system_out_writeLn_var).Groups[1].Value).Replace("$linenum", _lineCounter.ToString())); stopReasonIsEnd = false; break; }
                }
                #endregion


                #region Variables Parser
                //string
                if (Regex.IsMatch(line, pattern_string_var))
                {
                    string capture_name = Regex.Match(line, pattern_string_var).Groups[1].Value;
                    string capture_value = Regex.Match(line, pattern_string_var).Groups[2].Value;
                    if (capture_name == string.Empty)
                    {
                        SendError("\r\n" + Locale.VariableNameCannotBeNull.Replace("$linenum", _lineCounter.ToString()));
                        stopReasonIsEnd = false;
                        break;
                    }
                    else
                    {
                        switch (capture_value)
                        {
                            case "":
                                if (!_stringVars.ContainsKey(capture_name))
                                    _stringVars.Add(capture_name, "null");
                                else
                                {
                                    _stringVars.Remove(capture_name);
                                    _stringVars.Add(capture_name, "null");
                                }
                                break;
                            default:
                                if (!_stringVars.ContainsKey(capture_name))
                                    _stringVars.Add(capture_name, capture_value);
                                else
                                {
                                    _stringVars.Remove(capture_name);
                                    _stringVars.Add(capture_name, capture_value);
                                }
                                break;
                        }
                    }
                }
                #endregion


                _lineCounter++;
            }
            Stop(stopReasonIsEnd);
            
        }

        public void Stop(bool canceledByEnd)
        {
            if (ParsingStoppedEvent != null)
            {
                #region ParsingEventRunning Init

                Console.WriteLine("\r\n" + Locale.StateStopped);
                ParsingStoppedEventArgs _psea = new ParsingStoppedEventArgs();
                _psea.CanceledByScriptEnd = canceledByEnd;
                ParsingStoppedEvent(null, _psea);
                #endregion

            }
        }

        public static void SendError(string contents)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\n" + contents);
            Console.ResetColor();
        }
    }

    public class ParsingRunningEventArgs : EventArgs
    {
        [DefaultValue(false)]
        public bool IsRunning { get; set; }
    }

    public class ParsingStoppedEventArgs : EventArgs
    {
        [DefaultValue(false)]
        public bool CanceledByScriptEnd { get; set; }
    }
}
