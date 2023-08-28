using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTO_s.Users
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public int LastModifiedBy { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
    }
}
