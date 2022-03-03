using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    internal class LmsRepository : ILmsRepository
    {
        private LmsApiContext db;

        public LmsRepository(LmsApiContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void AddCourse(Course course)
        {
            db.Course.Add(course);
        }

        public void DeleteCourse(int id)
        {
            var course = db.Course.FirstOrDefault(c=>c.Id == id);
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            db.Course.Remove(course);

        }

        public IEnumerable<Course> GetAllCourses(string response, string sort)
        {
            if (response.ToUpper() == "Y")
            {
                var courses = db.Course.Include(c => c.Modules).AsQueryable();
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

        public Course GetCourseById(int id)
        {
            var course = db.Course.Include(c => c.Modules)
                .FirstOrDefault(c=>c.Id==id);
            return course;
        }

        public void UpdateCourse(Course course)
        {
            db.Entry(course).State = EntityState.Modified;
        }
        public Course PartialUpdateCourse(int id)
        {
           var courseobj = db.Course.FirstOrDefault(c=>c.Id==id);
            return courseobj;
        }
        public bool CourseExists(int id)
        {
            return db.Course.Any(e => e.Id == id);
        }

        public (IEnumerable<Module>,int) GetAllModules(string sort, int PageNumber, int PageSize)
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

        public IEnumerable<Module> GetAllModules(string title )
        {
            return db.Module.Where(c => c.Title == title).AsEnumerable();
        }

        public void UpdateModule(Module module)
        {
            db.Entry(module).State = EntityState.Modified;
        }

        public Module PartialUpdateModule(int id)
        {
            var module = db.Module.FirstOrDefault(m => m.Id == id);
            return module;
        }

        public void AddModule(Module module)
        {
            db.Module.Add(module);
        }

        public void DeleteModule(int id)
        {
            var module = db.Module.FirstOrDefault(m => m.Id == id);
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }
            db.Module.Remove(module);
        }

        public bool ModuleExists(int id)
        {
            return db.Module.Any(e => e.Id == id);
        }
    }
}
