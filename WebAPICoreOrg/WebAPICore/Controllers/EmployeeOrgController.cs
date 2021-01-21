using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPICore.Data;
using WebAPICore.Models;

namespace WebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeOrgController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeOrgController(EmployeeContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public IEnumerable<Employee> GetManager([FromRoute] long id)
        {
            return _context.Employees.FromSql<Employee>("uspGetManager {0}", id).ToList();

            //if (manager == null)
            //{
            //    manager.EmployeeName = "Not Found";
            //}
        }
    }
}