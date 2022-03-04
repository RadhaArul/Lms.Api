#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using AutoMapper;
using Lms.Core.Dto;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Lms.Core.Repositories;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public CoursesController(IMapper mapper, IUnitOfWork unitofwork)
        {
            this.mapper = mapper;
            this.uow = unitofwork;
        }

        // GET: api/Courses
        [HttpGet]
        // Filtering and Sorting 
        public async Task<ActionResult<IEnumerable<Course>>>
            GetCourse([FromQuery(Name ="Course_with_Module_Y/N")]string response="N", 
            [FromQuery(Name ="Title_ByAsc_A / D_for_desc")]string sort="A")
        {
            if (response.ToUpper() == "N")
            {
                var courses = await uow.LmsRepo.GetAllCourses(response, sort);
                return Ok( mapper.Map<IEnumerable<CourseGetDto>>(courses));
            }
            else
            {
                var courses = await uow.LmsRepo.GetAllCourses(response, sort);
                return Ok(mapper.Map<IEnumerable<CourseModuleGetDto>>(courses));
            }

        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseModuleGetDto>> GetCourse(int id)
        {
           
            var coursebyid = await uow.LmsRepo.GetCourseById(id);
           
            if (coursebyid == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CourseModuleGetDto>(coursebyid));
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CoursePutDto course)
        {
            

            if (!await uow.LmsRepo.CourseExists(id))
                return NotFound();

            var courseobj = mapper.Map<Course>(course);
            
         
            if (id != courseobj.Id)
            {
                return NotFound();
            }
            await uow.LmsRepo.UpdateCourse(courseobj);
           

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.LmsRepo.CourseExists(id))
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
        public async Task<ActionResult<Course>> PartialUpdateCourse(int courseId, JsonPatchDocument<Course> patchcourse)
        {
            var courseobj =  uow.LmsRepo.PartialUpdateCourse(courseId) ;
            patchcourse.ApplyTo(await courseobj);
            await uow.CompleteAsync();
            return NoContent();

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
                
                await uow.LmsRepo.AddCourse(courseobj);
                await uow.CompleteAsync();

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
            await uow.LmsRepo.DeleteCourse(id);
            await uow.CompleteAsync();
            return NoContent();
        }

        
    }
}
