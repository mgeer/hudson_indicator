namespace HudsonDaemon.Persistance
{
    using System;
    using System.IO;
    using System.Text;

    public class Repository
    {
        private const string REPO_FILE = "repo.hd";

        public string Load()
        {
            if (!File.Exists("repo.hd"))
            {
                return string.Empty;
            }
            using (StreamReader reader = new StreamReader("repo.hd", Encoding.UTF8))
            {
                string str = reader.ReadToEnd();
                reader.Close();
                return str;
            }
        }

        public void Save(string viewAsJson)
        {
            using (StreamWriter writer = new StreamWriter("repo.hd", false, Encoding.UTF8))
            {
                writer.Write(viewAsJson);
                writer.Flush();
                writer.Close();
            }
        }
    }
}

