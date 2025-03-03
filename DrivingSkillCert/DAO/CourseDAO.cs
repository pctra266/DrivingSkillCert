using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivingSkillCert.Models;

namespace DrivingSkillCert.DAO
{
    internal class CourseDAO
    {
        static DrivingSkillCertContext context = new DrivingSkillCertContext();
        public static List<Course> getCourses() => context.Courses.ToList();

    }
}
