using System.Runtime.Serialization;
using HudsonIndicator.HudsonDaemon.Hudson;

namespace HudsonIndicator.HudsonDaemon.Persistance
{
    [DataContract]
    public class JobLine
    {
        [DataMember]
        public JobItem Item { get; set; }

        [DataMember]
        public bool Selected { get; set; }
    }
}

