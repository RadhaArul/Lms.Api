using Lms.Core.Dto;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.Core;


namespace Lms.Core.Repositories
{
    public interface ILmsRepository
    {
        Task<IEnumerable<Course>> GetAllCourses(string response, string sort);
        Task<Course> GetCourseById(int id);
        Task UpdateCourse(Course course);
        Task AddCourse(Course course);  
        Task DeleteCourse(int id);
        Task<Course> PartialUpdateCourse(int id);
        Task<bool> CourseExists(int id);
        Task<(IEnumerable<Module>,int)> GetAllModules(string sort, int PageNumber, int PageSize);
        Task<IEnumerable<Module>> GetAllModules(string title);
        Task UpdateModule(Module module);
        Task<Module> PartialUpdateModule(int id);
        Task AddModule(Module module);
        Task DeleteModule(int id);
        Task<bool> ModuleExists(int id);
    }
}
