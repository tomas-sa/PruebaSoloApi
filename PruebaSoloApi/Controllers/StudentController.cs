using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaSoloApi.Data;
using PruebaSoloApi.DTOs;
using PruebaSoloApi.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PruebaSoloApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public readonly DataContext _context;

        public StudentController(DataContext context) 
        { 
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await _context.Students.Include(s => s.Courses).ToListAsync();

            var studentResponse = students.Select(student => new StudentWithCoursesDTO
            {
                Name = student.Name,
                Courses = student.Courses.Select(course => new CourseDTO
                {
                    Name = course.Name
                }).ToList()
            }).ToList();


            return Ok(studentResponse);
        }



        [HttpGet("{id}")]

        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound("No se encontró a un alumno con ese ID");
            }
            return Ok(student);
        }
        /*
        [HttpPost]

        public async Task<ActionResult<List<Student>>> AddStudent(StudentDTO request)
        {
            var student = new Student
            {
                Name = request.Name,
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            

            return Ok(await _context.Students.ToListAsync());


        }*/
        /*
        [HttpPost]
        public IActionResult PostStudent([FromBody] StudentDTO studentWithCoursesDTO)
        {
            if (studentWithCoursesDTO.CourseIds == null || !studentWithCoursesDTO.CourseIds.Any())
            {
                return BadRequest("Debes proporcionar al menos un ID de curso.");
            }

            var student = new Student
            {
                Name = studentWithCoursesDTO.Name,
                Courses = new List<Course>() // Inicializar la colección para evitar NullReferenceException
            };

            foreach (var courseId in studentWithCoursesDTO.CourseIds)
            {
                var course = _context.Courses.Find(courseId);
                if (course == null)
                {
                    return NotFound($"No se encontró el curso con ID {courseId}.");
                }

                student.Courses.Add(course);
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(student, jsonOptions);

            return CreatedAtAction("GetStudent", new { id = student.Id }, json);
        }
        */
        [HttpPost]
        public async Task<ActionResult<StudentWithCoursesDTO>>PostStudent([FromBody] StudentDTO studentWithCoursesDTO)
        {
            if (studentWithCoursesDTO.CourseIds == null || !studentWithCoursesDTO.CourseIds.Any())
            {
                return BadRequest("Debes proporcionar al menos un ID de curso.");
            }

            var student = new Student
            {
                Name = studentWithCoursesDTO.Name,
                Courses = new List<Course>() // Inicializar la colección para evitar NullReferenceException
            };

            var coursesDb = new List<CourseDTO>();

            foreach (var courseId in studentWithCoursesDTO.CourseIds)
            {
                var course = _context.Courses.Find(courseId);
                if (course == null)
                {
                    return NotFound($"No se encontró el curso con ID {courseId}.");
                }

                student.Courses.Add(course);
                coursesDb.Add(new CourseDTO { Name = course.Name});
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            var studentDTO = new StudentWithCoursesDTO {
                Name = studentWithCoursesDTO.Name,
                Courses = coursesDb
            };


            return Ok(studentDTO);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Student>> PutStudent(StudentDTO student, int id)
        {
            var DbStudent = await _context.Students.FindAsync(id);
            if(DbStudent == null)
            {
                return NotFound("No se encontró al alumno");
            }

            DbStudent.Name = student.Name;
            await _context.SaveChangesAsync();
            return Ok(await _context.Students.FindAsync(id));
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<List<Student>>> DeleteStudent(int id)
        {
            var dbStudent = await _context.Students.FindAsync(id);
            if(dbStudent == null)
            {
                return NotFound("No se encontró al alumno");
            }
            _context.Students.Remove(dbStudent);
            await _context.SaveChangesAsync();

            return Ok(await _context.Students.ToListAsync());
        }
    }
}
