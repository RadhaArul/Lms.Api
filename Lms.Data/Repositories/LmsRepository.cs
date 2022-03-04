using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories
{
    internal class LmsRepository : ILmsRepository
    {
        private LmsApiContext db;

        public LmsRepository(LmsApiContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddCourse(Course course)
        {
             await  db.Course.AddAsync(course);
        }

        public async Task DeleteCourse(int id)
        {
            var course = await db.Course.FirstOrDefaultAsync(c=>c.Id == id);
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            db.Course.Remove(course);

        }

        public async Task<IEnumerable<Course>> GetAllCourses(string response, string sort)
        {
            if (response.ToUpper() == "Y")
            {
                var courses =  db.Course.Include(c => c.Modules).AsQueryable();
                switch (sort.ToUpper())
                {
                    case "A":
                        {
                            courses =  courses.OrderBy(x => x.Title);
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
                return  courses;
            }
            else
            {
                var courses = db.Course.AsQueryable();  
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
                return courses;
            }
        }

        public async Task<Course> GetCourseById(int id)
        {
            var course =  await db.Course.Include(c => c.Modules)
                .FirstOrDefaultAsync(c=>c.Id==id);
            return (course);
        }

        public async Task UpdateCourse(Course course)
        {
            db.Entry(course).State = EntityState.Modified;
        }
        public async Task<Course> PartialUpdateCourse(int id)
        {
           var courseobj = await db.Course.FirstOrDefaultAsync(c=>c.Id==id);
            return courseobj;
        }
        public async Task<bool> CourseExists(int id)
        {
            return db.Course.Any(e => e.Id == id);
        }

        public async Task<(IEnumerable<Module>,int)> GetAllModules(string sort, int PageNumber, int PageSize)
        {
            var modules = db.Module.AsQueryable();
                if (sort.ToUpper() == "A")
                modules = modules.OrderBy(x => x.Title);
            else
                modules = modules.OrderByDescending(x => x.Title);

            int totalItemCount = modules.Count();
            

            modules = modules.Skip(PageSize * (PageNumber - 1)).Take(PageSize);

            return (modules,totalItemCount);
        }

        public async Task<IEnumerable<Module>> GetAllModules(string title )
        {
            return db.Module.Where(c => c.Title == title).AsEnumerable();
        }

        public async Task UpdateModule(Module module)
        {
            db.Entry(module).State = EntityState.Modified;
        }

        public async Task<Module> PartialUpdateModule(int id)
        {
            var module = await db.Module.FirstOrDefaultAsync(m => m.Id == id);
            return module;
        }

        public async Task AddModule(Module module)
        {
            await db.Module.AddAsync(module);
        }

        public async Task DeleteModule(int id)
        {
            var module = await db.Module.FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }
            db.Module.Remove(module);
        }

        public async Task<bool> ModuleExists(int id)
        {
            return db.Module.Any(e => e.Id == id);
        }
    }
}
