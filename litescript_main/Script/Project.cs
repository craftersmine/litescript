using System.IO;

using craftersmine.Config;

namespace craftersmine.LiteScript.Script
{
    public class Project
    {
        private Configuration _project;

        public string ProjectFilename { get; }
        public string ScriptFilename { get; set; }
        public string ProjectRootFolder { get; set; }
        public string ScriptName { get; set; }

        public Project(string file)
        {
            ProjectFilename = file;
            string _ext = Path.GetExtension(ProjectFilename);
            if (_ext == ".lsproj")
            {
                _project = new Configuration(file, false);  // Подгружаем конфигурацию проекта
                ScriptName = _project.GetString("project.scriptname");
                ScriptFilename = _project.GetString("project.scriptfilename");
                ProjectRootFolder = _project.GetString("project.root");
            }
        }

        public string GetScriptName()
        {
            ScriptName = _project.GetString("project.scriptname");
            return ScriptName;
        }

        public string GetScriptFilename()
        {
            ScriptFilename = _project.GetString("project.scriptfilename");
            return ScriptFilename;
        }

        public string GetProjectRootFolder()
        {
            ProjectRootFolder = _project.GetString("project.root");
            return ProjectRootFolder;
        }

        public ScriptFile GetScriptFile()
        {
            return new ScriptFile(ProjectRootFolder + ScriptFilename);
        }
    }
}
