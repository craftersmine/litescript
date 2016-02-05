using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;

using craftersmine.LiteScript.Cfg.Console;
using craftersmine.LiteScript.Locale.Console;

using craftersmine.LiteScript.Parser.Patterns;

namespace craftersmine.LiteScript.Parser
{
    /// <summary>
    /// Main parser core. This class can not be inherited
    /// </summary>
    public sealed class ParserCore
    {
        #region Delegates
        /// <summary>
        /// Parse Running State Event Delegate
        /// </summary>
        /// <param name="sender">Event caller</param>
        /// <param name="e">Event arguments</param>
        public delegate void ParsingRunningEventDelegate(object sender, ParsingRunningEventArgs e);
        /// <summary>
        /// Parse Stopped State Event Delegate
        /// </summary>
        /// <param name="sender">Event caller</param>
        /// <param name="e">Event arguments</param>
        public delegate void ParsingStoppedEventDelegate(object sender, ParsingStoppedEventArgs e);
        #endregion

        #region Events
        /// <summary>
        /// Parse Running State Event
        /// </summary>
        public event ParsingRunningEventDelegate ParsingRunningEvent;
        /// <summary>
        /// Parse Running State Event
        /// </summary>
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

        public static bool StopReasonIsEnd = true;
        public static int LineCounter;

        #region Variable DB
        public static Dictionary<string, string> StringVars = new Dictionary<string, string>();
        public static Dictionary<string, int> IntVars = new Dictionary<string, int>();
        #endregion
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
            LineCounter = 1 + CommentLinesCounter;

            foreach (var line in _scriptContents)
            {

                #region system:
                #region system:out:write
                #region normaltext
                if (Regex.IsMatch(line, SystemPatterns.SystemOutWrite))
                    Console.Write(Regex.Match(line, SystemPatterns.SystemOutWrite).Groups[1].Value);
                #endregion
                #region variable val
                if (Regex.IsMatch(line, SystemPatterns.SystemOutWriteVar))
                {
                    if (StringVars.ContainsKey(Regex.Match(line, SystemPatterns.SystemOutWriteVar).Groups[1].Value))
                    {
                        string val;
                        StringVars.TryGetValue(Regex.Match(line, SystemPatterns.SystemOutWriteVar).Groups[1].Value, out val);
                        Console.Write(val);
                    }
                    else { SendError(Locale.VariableNotInitialized.Replace("$name", Regex.Match(line, SystemPatterns.SystemOutWriteVar).Groups[1].Value).Replace("$linenum", ParserCore.LineCounter.ToString())); StopReasonIsEnd = false; break; }
                }
                #endregion
                #endregion

                #region system:out:write@Ln
                #region normaltext
                if (Regex.IsMatch(line, SystemPatterns.SystemOutWriteLn))
                {
                    string capture = Regex.Match(line, SystemPatterns.SystemOutWriteLn).Groups[1].Value;
                    if (capture == string.Empty)
                        Console.WriteLine();
                    else
                        Console.WriteLine(capture);
                }
                #endregion

                #region variable val
                if (Regex.IsMatch(line, SystemPatterns.SystemOutWriteLnVar))
                {
                    if (StringVars.ContainsKey(Regex.Match(line, SystemPatterns.SystemOutWriteLnVar).Groups[1].Value))
                    {
                        string val;
                        StringVars.TryGetValue(Regex.Match(line, SystemPatterns.SystemOutWriteLnVar).Groups[1].Value, out val);
                        if (val == "null")
                            Console.WriteLine("\r\n");
                        else
                            Console.WriteLine(val);
                    }
                    else { SendError(Locale.VariableNotInitialized.Replace("$name", Regex.Match(line, SystemPatterns.SystemOutWriteLnVar).Groups[1].Value).Replace("$linenum", LineCounter.ToString())); StopReasonIsEnd = false; break; }


                }
                #endregion
                #endregion

                #region system:setTitle
                if (Regex.IsMatch(line, SystemPatterns.SystemSetTitle))
                    Console.Title = Regex.Match(line, SystemPatterns.SystemSetTitle).Groups[1].Value;
                #endregion

                #region system:in:read
                if (Regex.IsMatch(line, SystemPatterns.SystemInRead))
                {
                    string varNameCapture = Regex.Match(line, SystemPatterns.SystemInRead).Groups[1].Value;
                    Console.WriteLine();
                    Console.Write("> ");
                    string readed = Console.ReadLine().Replace("> ", "");
                    if (StringVars.ContainsKey(varNameCapture))
                    {
                        StringVars.Remove(varNameCapture);
                        if (readed == string.Empty)
                            StringVars.Add(varNameCapture, "null");
                        else StringVars.Add(varNameCapture, readed);
                    }
                    else { SendError(Locale.VariableNotInitialized.Replace("$name", Regex.Match(line, SystemPatterns.SystemInRead).Groups[1].Value).Replace("$linenum", LineCounter.ToString())); StopReasonIsEnd = false; break; }
                }
                #endregion
                #endregion
                
                #region Variables Parser
                #region string
                if (Regex.IsMatch(line, SystemPatterns.StringVar))
                {
                    string capture_name = Regex.Match(line, SystemPatterns.StringVar).Groups[1].Value;
                    string capture_value = Regex.Match(line, SystemPatterns.StringVar).Groups[2].Value;
                    if (capture_name == string.Empty)
                    {
                        SendError("\r\n" + Locale.VariableNameCannotBeNull.Replace("$linenum", LineCounter.ToString()));
                        StopReasonIsEnd = false;
                        break;
                    }
                    else
                    {
                        switch (capture_value)
                        {
                            case "":
                                if (!StringVars.ContainsKey(capture_name))
                                    StringVars.Add(capture_name, "null");
                                else
                                {
                                    StringVars.Remove(capture_name);
                                    StringVars.Add(capture_name, "null");
                                }
                                break;
                            default:
                                if (!StringVars.ContainsKey(capture_name))
                                    StringVars.Add(capture_name, capture_value);
                                else
                                {
                                    StringVars.Remove(capture_name);
                                    StringVars.Add(capture_name, capture_value);
                                }
                                break;
                        }
                    }
                }
                #endregion
                #endregion

                #region fs:

                

                #endregion

                LineCounter++;
            }
            Stop(StopReasonIsEnd);
            
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
