using System.IO;
using System.Text;

namespace HudsonIndicator.HudsonDaemon.Persistance
{
    public class Repository
    {
        private const string RepoFile = "repo.hd";

        public string Load()
        {
            if (!File.Exists("repo.hd"))
            {
                return string.Empty;
            }
            using (var reader = new StreamReader(RepoFile, Encoding.UTF8))
            {
                var str = reader.ReadToEnd();
                reader.Close();
                return str;
            }
        }

        public void Save(string viewAsJson)
        {
            using (var writer = new StreamWriter(RepoFile, false, Encoding.UTF8))
            {
                writer.Write(viewAsJson);
                writer.Flush();
                writer.Close();
            }
        }
    }
}

