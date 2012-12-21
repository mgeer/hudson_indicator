using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HudsonIndicator.HudsonDaemon.Hudson
{
    [DataContract]
    public class JobDetail
    {
        public static JobDetail Parse(string jobDetailInJson)
        {
            var serializer = new DataContractJsonSerializer(typeof(JobDetail));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jobDetailInJson)))
            {
                return (JobDetail) serializer.ReadObject(stream);
            }
        }

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string description { get; set; }
    }
}

