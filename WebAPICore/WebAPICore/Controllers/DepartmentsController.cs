using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPICore.Data;
using WebAPICore.Models;

namespace WebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public DepartmentsController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        //[HttpGet]
        //public IEnumerable<Department> GetDepartments()
        //{
        //    return _context.Departments;
        //}


        //[HttpGet]
        //public IEnumerable<Department> GetDepartments()
        //{
        //    return _context.Departments.FromSql<Department>("getDepartment").ToList();
        //}

        [HttpGet]
        public IEnumerable<DepartmentWithEmployeeCount> GetDepartments()
        {
            return _context.departmentWithEmployees.FromSql<DepartmentWithEmployeeCount>("spGetDepartmentWithEmployeeCount").ToList();
        }

        // GET: api/Departments/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetDepartment([FromRoute] long id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var department = await _context.Departments.FindAsync(id);


        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(department);
        //}

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public Department GetDepartment([FromRoute] long id)
        {

            var department = _context.Departments.FromSql<Department>("spGetDepartmentById {0}",id).ToList().FirstOrDefault();

            if(department == null)
            {
                department.DepartmentName = "Not Found";
            }
            return department;
        }

        /*[FromRoute] long id,*/
        // PUT: api/Departments/5
        [HttpPut]
        public async Task<IActionResult> PutDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != department.DepartmentID)
            //{
            //    return BadRequest();
            //}

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!DepartmentExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
                throw;
            }

            return Content("Updated Successfully");
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> PostDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.DepartmentID }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        private bool DepartmentExists(long id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}