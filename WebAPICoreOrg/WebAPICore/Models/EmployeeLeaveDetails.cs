using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Models
{
    public class EmployeeLeaveDetails
    {
        public long LeaveID { get; set; }
        public long EmployeeID { get; set; }
        public string LeaveType { get; set; }
        public DateTime? LStartDate { get; set; }
        public DateTime? LEndDate { get; set; }
        public long LeaveDays { get; set; }
        public string DocPath { get; set; }
        public string LeaveStatus { get; set; }
        public string EmployeeName { get; set; }
    }
}
