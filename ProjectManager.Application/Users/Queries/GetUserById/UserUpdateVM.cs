using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Queries.GetUserById
{
    public class UserUpdateVM
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Tuple<int, string>> Projects { get; set; } = new List<Tuple<int, string>>();
        public List<Tuple<int, Tuple<string, string>>> ProjectTasks { get; set; } = new List<Tuple<int, Tuple<string, string>>>();
    }
}
