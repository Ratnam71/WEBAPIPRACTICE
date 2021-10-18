using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI_PRACTICE.Models;


namespace WebAPI_PRACTICE.Controllers
{
    public class StudentsController : ApiController
    {
        //GET - Retrieve a Data
        public IHttpActionResult GetAllStudents()
        {
            IList<Students_Model> students = null;
            using (var v = new WebApiPractice_DBEntities())
            {
                students = v.Students_Details
                             .Select(c => new Students_Model()
                             {
                                 Id = c.id,
                                 Name = c.name,
                                 Email = c.email,
                                 Address = c.address,
                                 Phone = c.phone
                             }).ToList<Students_Model>();

            }
            if (students.Count == 0)
                return NotFound();
            return Ok(students);
        }
        //Post -Add a new Record
        public IHttpActionResult PostNewStudent(Students_Model student)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data ,Please Add again");
            using (var v =new WebApiPractice_DBEntities())
            {
                v.Students_Details.Add(new Students_Details()
                {
                    name = student.Name,
                    email = student.Email,
                    address = student.Address,
                    phone = student.Phone
                });
                v.SaveChanges();
            }

            return Ok();
        }
        //PUT -Updates the Entire Record
        public IHttpActionResult PutStudents(Students_Model student)
        {
            if (!ModelState.IsValid)
                return BadRequest("this is invalid,Please recheck");
            using (var v = new WebApiPractice_DBEntities())
            {
                var checkExustingStudent = v.Students_Details.Where(c=> c.id == student.Id)
                                                               .FirstOrDefault<Students_Details>();
                if (checkExustingStudent != null)
                {
                    checkExustingStudent.name = student.Name;
                    checkExustingStudent.address = student.Address;
                    checkExustingStudent.phone = student.Phone;

                    v.SaveChanges();

                }
                else
                    return NotFound();
                
            }
            return Ok();

        }
        //Delete - delecte a record based on the Id
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Please enter a valid student Id");
            using (var v= new WebApiPractice_DBEntities())
            {
                var students = v.Students_Details
                                .Where(c => c.id == id)
                                .FirstOrDefault();
                v.Entry(students).State = System.Data.Entity.EntityState.Deleted;
                v.SaveChanges();
            }
            return Ok();
        }



    }
}
