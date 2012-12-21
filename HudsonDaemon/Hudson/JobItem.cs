using System.Runtime.Serialization;

namespace HudsonIndicator.HudsonDaemon.Hudson
{
    [DataContract]
    public class JobItem
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string url { get; set; }
    }
}

