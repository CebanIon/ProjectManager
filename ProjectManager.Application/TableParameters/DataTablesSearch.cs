using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ProjectManager.Application.TableParameters
{
    [Serializable]
    [DataContract]
    public class DataTablesSearch
    {
        public DataTablesSearch()
        {
            Values = new List<string>();
        }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        public ICollection<string> Values { get; set; }

        [DataMember(Name = "regex")]
        public string Regex { get; set; }
    }
}
