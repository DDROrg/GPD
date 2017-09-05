using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "application")]
    public class ApplicationDTO
    {
        [DataMember(Name = "build", Order = 1)]
        public string Build;

        [DataMember(Name = "client-ip", Order = 2)]
        public string ClientIP;

        [DataMember(Name = "name", Order = 3)]
        public string Name;

        [DataMember(Name = "plugin-build", Order = 4)]
        public string PluginBuild;

        [DataMember(Name = "plugin-source", Order = 5)]
        public string PluginSource;

        [DataMember(Name = "plugin-name", Order = 6)]
        public string PluginName;

        [DataMember(Name = "type", Order = 7)]
        public string Type;

        [DataMember(Name = "username", Order = 8)]
        public string Username;

        [DataMember(Name = "version", Order = 9)]
        public string Version;
    }
}
