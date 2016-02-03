using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.LiteScript.Parser.Patterns
{
    public class SystemPatterns
    {
        //system
        public static string SystemOutWrite = @"system:out:write\[""(.*)""\]";
        public static string SystemOutWriteLn = @"system:out:write@Ln\[""(.*)""\]";
        public static string SystemSetTitle = @"system:setTitle\[""(.*)""\]";

        public static string SystemOutWriteVar = @"system:out:write\[\$([\w\s].*)=>getValue\]";
        public static string SystemOutWriteLnVar = @"system:out:write@Ln\[\$([\w\s].*)=>getValue\]";

        public static string SystemInRead = @"system:in:read\[\$(.*)<=setValue\]";


        //variables
        public static string StringVar = @"string\$(.*)<=setValue\[""(.*)""\]";
    }
}
