#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using AutoMapper;
using Lms.Core.Dto;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;

        public CoursesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        // Filtering and Sorting 
        public async Task<ActionResult<IEnumerable<Course>>>
            GetCourse([FromQuery(Name ="Do_you_want_Course_with_Module_Y/N")]string response="N", 
            [FromQuery(Name ="Sorting_By_Title_enter_A_for_asc / D_for_desc")]string sort="A")
        {
            // var courses = _context.Course.Include(c => c.Modules);
            if ( response.ToUpper() == "Y")
            {
                 var courses = mapper.ProjectTo<CourseModuleGetDto>(_context.Course);
                switch(sort.ToUpper())
                {
                    case "A":
                        {
                            courses=courses.OrderBy(x => x.Title);
                            break;
                        }
                        case "D":
                        {
                            courses = courses.OrderByDescending(x => x.Title);
                            break ;
                        }
                    default:
                        break;

                }
                return Ok(courses);
            }
            else
            { 
                 var courses = mapper.ProjectTo<CourseGetDto>(_context.Course);
                switch (sort.ToUpper())
                {
                    case "A":
                        {
                            courses = courses.OrderBy(x => x.Title);
                            break;
                        }
                    case "D":
                        {
                            courses = courses.OrderByDescending(x => x.Title);
                            break;
                        }
                    default:
                        break;

                }
                return Ok(courses);
            }
            
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            //var course =await _context.Course.Include(c => c.Modules)
            //    .FirstOrDefaultAsync(c=>c.Id==id);
           

            var course = mapper.ProjectTo<CourseModuleGetDto>(_context.Course).FirstOrDefault(c=>c.Id==id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CoursePutDto course)
        {
            

            if (!CourseExists(id))
                return NotFound();

            var courseobj = mapper.Map<Course>(course);
            
         
            if (id != courseobj.Id)
            {
                return NotFound();
            }

            _context.Entry(courseobj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }

            return NoContent();
        }
        // PartialPUT: api/Courses/5
        [HttpPatch("{courseId}")]
        public async Task<IActionResult> PartialUpdateCourse(int courseId, JsonPatchDocument<Course> patchcourse)
        {
            var courseobj =await _context.Course.FindAsync(courseId);
            
            patchcourse.ApplyTo(courseobj);
            await _context.SaveChangesAsync();
            return StatusCode(200);

        }


        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CoursePostDto course)
        {
            if (course == null)
            {
                return NotFound();
            }

            var courseobj=mapper.Map<Course>(course);

            try
            {
                _context.Course.Add(courseobj);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCourse", new { id = course.Id }, courseobj);
            }
            catch 
            {
                return StatusCode(500);
            }


            
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
