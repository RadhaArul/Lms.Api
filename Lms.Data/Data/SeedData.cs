using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        private static Faker faker = null!;
        private static Random random = new Random();
        public static async Task InitAsync(LmsApiContext db)
        {
            if (await db.Course.AnyAsync()) return;

            faker = new Faker("sv");

            var courses = GetCourses();
            await db.Course.AddRangeAsync(courses);

            //var modules = GetModules(courses);
            //await db.Module.AddRangeAsync(modules);

            await db.SaveChangesAsync();


        }

        //private static IEnumerable<Module> GetModules(IEnumerable<Course> courses)
        //{
        //    var modules = new List<Module>();
            
        //    foreach (var course in courses)
        //    {
        //        for(int i=0; i<3; i++)
        //        {
        //            var moduleobj = new Module();
        //            moduleobj.Title = faker.Company.CatchPhrase();
        //            moduleobj.StartDate = course.StartDate.AddMonths(random.Next(4));
        //            //moduleobj.CourseId = course.Id;
        //            modules.Add(moduleobj);
        //        }
                
        //    }
        //    return modules;
        //}

        private static IEnumerable<Course> GetCourses()
        {
            var courses = new List<Course>();
            
            for (int i = 0; i < 5; i++)
            {
                var courseobj = new Course();
                var modules =new List<Module>();
                courseobj.Title = faker.Company.CatchPhrase();
                courseobj.StartDate = faker.Date.Recent(20);
                for (int j = 0; j < 3; j++)
                {
                    var moduleobj = new Module();
                    moduleobj.Title = faker.Company.CatchPhrase();
                    moduleobj.StartDate = courseobj.StartDate.AddMonths(random.Next(4));
                    //moduleobj.CourseId = course.Id;
                    modules.Add(moduleobj);
                }
                courseobj.Modules = modules;
                courses.Add(courseobj);
            }
            return courses;

        }
    }
}
