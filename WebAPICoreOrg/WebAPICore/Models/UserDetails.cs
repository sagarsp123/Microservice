using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Models
{
    public class UserDetails
    {
        [Key]
        public long UserID { get; set; }
        public string EmailID { get; set; }
        public string ProfilePassword { get; set; }
        public string UserRole { get; set; }
    }
}
