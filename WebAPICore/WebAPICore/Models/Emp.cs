using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Models
{
    public class Emp
    {
        public long id { get; set; }
        public string employeeName { get; set; }
        public string department { get; set; }
        public string mailID { get; set; }
        public DateTime? doj { get; set; }

        public int? pid { get; set; }
    }
}
