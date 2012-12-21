using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HudsonIndicator.HudsonDaemon.Persistance
{
    [DataContract]
    public class DaemonView
    {
        public static DaemonView Parse(string viewAsJson)
        {
            var serializer = new DataContractJsonSerializer(typeof(DaemonView));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(viewAsJson)))
            {
                return (DaemonView) serializer.ReadObject(stream);
            }
        }

        public static string Serialize(DaemonView view)
        {
            string str;
            var serializer = new DataContractJsonSerializer(typeof(DaemonView));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, view);
                stream.Position = 0L;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    str = reader.ReadToEnd();
                }
            }
            return str;
        }

        [DataMember]
        public JobLine[] JobLines { get; set; }

        [DataMember]
        public string[] Urls { get; set; }
    }
}

