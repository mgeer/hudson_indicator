namespace HudsonDaemon.Hudson
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    [DataContract]
    public class JobItems
    {
        public static JobItems Parse(string jobsInJson)
        {
            var serializer = new DataContractJsonSerializer(typeof(JobItems));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jobsInJson)))
            {
                return (JobItems) serializer.ReadObject(stream);
            }
        }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public JobItem[] jobs { get; set; }
    }
}

