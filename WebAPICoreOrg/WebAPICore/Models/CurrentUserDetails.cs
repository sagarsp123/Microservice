using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Models
{
    public class CurrentUserDetails
    {
        public long UserID { get; set; }
        public string EmailID { get; set; }
        public string ProfilePassword { get; set; }
        public string UserRole { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
       
    }
}
