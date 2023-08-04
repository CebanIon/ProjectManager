using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ProjectManager.Application.TableParameters
{
    [Serializable]
    [DataContract]
    public class DataTablesOrder
    {
        [DataMember(Name = "column")]
        public int Column { get; set; }

        [DataMember(Name = "dir")]
        public string Dir { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
