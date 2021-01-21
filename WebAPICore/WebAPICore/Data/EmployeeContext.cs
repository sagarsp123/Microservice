using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPICore.Models;

namespace WebAPICore.Data
{
    public class EmployeeContext: DbContext
    {
        public EmployeeContext(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<UserDetails> Userdetails { get; set; }
        //public DbSet<LeaveDetails> LeaveDetails { get; set; }
        //public DbQuery<CurrentUserDetails> CurrentUsers { get; set; }
        //public DbQuery<EmployeeLeaveDetails> EmployeeLeaveDetails { get; set; }
        public DbQuery<DepartmentWithEmployeeCount> departmentWithEmployees { get; set; }

    }
}
