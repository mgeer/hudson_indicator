namespace HudsonDaemon.Persistance
{
    using HudsonDaemon.Hudson;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobLine
    {
        [DataMember]
        public JobItem Item { get; set; }

        [DataMember]
        public bool Selected { get; set; }
    }
}

