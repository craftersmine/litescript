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
        public const string SystemOutWrite = @"system:out:write\[""(.*)""\]";
        public const string SystemOutWriteLn = @"system:out:write@Ln\[""(.*)""\]";
        public const string SystemSetTitle = @"system:setTitle\[""(.*)""\]";

        public const string SystemOutWriteVar = @"system:out:write\[\$([\w\s].*)=>getValue\]";
        public const string SystemOutWriteLnVar = @"system:out:write@Ln\[\$([\w\s].*)=>getValue\]";

        public const string SystemInRead = @"system:in:read\[\$(.*)<=setValue\]";


        //variables
        public const string StringVar = @"string\$(.*)<=setValue\[""(.*)""\]";
    }

    public class FSPatterns
    {
        public const string FSCreateFile = @"fs:createFile\[""(.*)""\]";
        public const string FSWriteText = @"fs:writeText\[""(.*)"",[\s\S]""(.*)""\]";
        public const string FSWriteTextVar = @"fs:writeText\[""(.*)"",[\s\S](.*)\]";
        public const string FSDeleteFile = @"fs:deleteFile\[""(.*)""\]";
    }
}
