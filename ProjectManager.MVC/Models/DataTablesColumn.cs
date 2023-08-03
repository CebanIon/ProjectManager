using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ProjectManager.MVC.Models
{
    [Serializable]
    [DataContract]
    public class DataTablesColumn
    {
        [DataMember(Name = "data")]
        public string Data { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "searchable")]
        public bool Searchable { get; set; }

        [DataMember(Name = "orderable")]
        public bool Orderable { get; set; }

        [DataMember(Name = "search")]
        public DataTablesSearch Search { get; set; }
    }
}
