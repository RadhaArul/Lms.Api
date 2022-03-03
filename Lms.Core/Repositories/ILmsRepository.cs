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
        IEnumerable<Course> GetAllCourses(string response, string sort);
        Course GetCourseById(int id);
        void UpdateCourse(Course course);
        void AddCourse(Course course);  
        void DeleteCourse(int id);
        Course PartialUpdateCourse(int id);
        bool CourseExists(int id);
        (IEnumerable<Module>,int) GetAllModules(string sort, int PageNumber, int PageSize);
        IEnumerable<Module> GetAllModules(string title);
        void UpdateModule(Module module);
        Module PartialUpdateModule(int id);
        void AddModule(Module module);
        void DeleteModule(int id);
        bool ModuleExists(int id);
    }
}
