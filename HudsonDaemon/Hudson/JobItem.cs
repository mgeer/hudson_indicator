namespace HudsonDaemon.Hudson
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JobItem
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string url { get; set; }
    }
}

