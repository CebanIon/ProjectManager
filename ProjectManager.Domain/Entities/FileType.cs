using ProjectManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class FileType : BaseEntity
    {
       public string Type { get; set; }
    }
}
